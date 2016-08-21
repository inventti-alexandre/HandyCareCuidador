using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCareCuidador.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using HandyCareCuidador.Interface;
using HandyCareCuidador;

namespace HandyCareCuidador.Data
{
    public class MaterialUtilizadoRestService : IMaterialUtilizadoRestService
    {
        static MaterialUtilizadoRestService defaultInstance = new MaterialUtilizadoRestService();
        MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<MaterialUtilizado> MaterialUtilizadoTable;
#else
        IMobileServiceTable<MaterialUtilizado> MaterialUtilizadoTable;
#endif
        public MaterialUtilizadoRestService()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);
            MaterialUtilizadoTable = client.GetTable<MaterialUtilizado>();
        }

        public bool IsOfflineEnabled
        {
            get { return MaterialUtilizadoTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<MaterialUtilizado>; }
        }

        public async Task DeleteMaterialUtilizadoAsync(MaterialUtilizado MaterialUtilizado)
        {
            await MaterialUtilizadoTable.DeleteAsync(MaterialUtilizado);
        }

        public async Task<ObservableCollection<MaterialUtilizado>> RefreshDataAsync(string Id, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif

                IEnumerable<MaterialUtilizado> items = await MaterialUtilizadoTable
                    .Where(m=>m.MatAfazer==Id)
                    .ToEnumerableAsync();
                return new ObservableCollection<MaterialUtilizado>(items);
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

        public async Task SaveMaterialUtilizadoAsync(MaterialUtilizado item, bool isNewItem)
        {
            try
            {
                if (isNewItem)
                {
                    await MaterialUtilizadoTable.InsertAsync(item);
                }
                else
                {
                    await MaterialUtilizadoTable.UpdateAsync(item);
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
    }
#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.MaterialUtilizadoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allMaterialUtilizado",
                    this.MaterialUtilizadoTable.CreateQuery());
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