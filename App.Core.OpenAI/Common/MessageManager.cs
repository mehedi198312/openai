namespace App.Core.OpenAI.Common
{
    public class MessageManager
    {
        public static string PinecoreConnectionFail
        {
            get
            {
                const string val = "File to connect with pinecone.";
                return val;
            }
        }

        public static string VectorSaveSuccessMessage
        {
            get
            {
                const string val = "Vector inserted successfully.";
                return val;
            }
        }

        public static string PinecoreIndexNotFound
        {
            get
            {
                const string val = "Pinecore index name not found.";
                return val;
            }
        }

        public static string InformationFound
        {
            get
            {
                const string val = "Information found successfully.";
                return val;
            }
        }

        public static string InformationDeleted
        {
            get
            {
                const string val = "Information delete successfully.";
                return val;
            }
        }

        public static string FileUploadFailed
        {
            get
            {
                const string val = "File fail to upload.";
                return val;
            }
        }

        public static string FileUploadSuccessfully
        {
            get
            {
                const string val = "File uploaded successfully.";
                return val;
            }
        }

        public static string ChunkCreateFailed
        {
            get
            {
                const string val = "Chunk fail to create.";
                return val;
            }
        }

        public static string ChunkCreateSuccessfully
        {
            get
            {
                const string val = "Chunk created successfully.";
                return val;
            }
        }

        public static string EmbeddingCreateFailed
        {
            get
            {
                const string val = "Embedding fail to create.";
                return val;
            }
        }

        public static string EmbeddingCreateSuccessfully
        {
            get
            {
                const string val = "Embedding created successfully.";
                return val;
            }
        }
   
    }
}
