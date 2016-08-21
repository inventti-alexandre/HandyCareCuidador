//#define OFFLINE_SYNC_ENABLED
using HandyCareCuidador.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCareCuidador.Model;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.MobileServices;
using System.Diagnostics;
#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace HandyCareCuidador.Data
{
    public class MaterialRestService : IMaterialRestService
    {
        static MaterialRestService defaultInstance = new MaterialRestService();
        MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<Material> MaterialTable;
#else
        IMobileServiceTable<Material> MaterialTable;
#endif
        public MaterialRestService()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);
#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<Material>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            MaterialTable = client.GetSyncTable<Material>();
#else
            MaterialTable = client.GetTable<Material>();
#endif
        }
        public async Task DeleteMaterialAsync(Material material)
        {
            try
            {
                await MaterialTable.DeleteAsync(material);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
        }

        public async Task<ObservableCollection<Material>> RefreshDataAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                IEnumerable<Material> items = await MaterialTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Material>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;

        }
        public async Task<ObservableCollection<Material>> RefreshDataExistenteAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                IEnumerable<Material> items = await MaterialTable
                    .Where(m => m.MatQuantidade > 0)
                    .ToEnumerableAsync();
                return new ObservableCollection<Material>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }
        public async Task SaveMaterialAsync(Material item, bool isNewItem)
        {
            try
            {
                if (isNewItem)
                {
                    await MaterialTable.InsertAsync(item);
                }
                else
                {
                    await MaterialTable.UpdateAsync(item);
                }
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
        }
#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.CuidadorTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allCuidador",
                    this.CuidadorTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
#endif
    }
}
