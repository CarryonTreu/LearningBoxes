using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace LearningBoxes.Helper {
    class ModelHelper {

        public async static void CreateFile(string fileName, object objectToStore) {
            //Create XML
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            //TODO dont ReplaceExisting but fail and return false
            StorageFile deckFile = await localFolder.CreateFileAsync(fileName + ".xml",
                CreationCollisionOption.ReplaceExisting);

            //Save Data to XML
            IRandomAccessStream raStream = await deckFile.OpenAsync(FileAccessMode.ReadWrite);
            using (IOutputStream outStream = raStream.GetOutputStreamAt(0)) {
                // Serialize the Session State. 
                DataContractSerializer serializer = new DataContractSerializer((objectToStore.GetType()));
                serializer.WriteObject(outStream.AsStreamForWrite(), objectToStore);
                await outStream.FlushAsync();
                outStream.Dispose();
                raStream.Dispose();
            }
        }
    }
}
