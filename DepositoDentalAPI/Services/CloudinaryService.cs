using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace DepositoDentalAPI.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        public Cloudinary cloudinary;
        private readonly IConfiguration config;
        public string CLOUD_NAME = System.String.Empty;
        public string API_KEY = System.String.Empty;
        public string API_SECRET = System.String.Empty;
       

        public CloudinaryService(IConfiguration config)
        {
            this.config = config;
            InicializarValores();
            Account account = new Account(CLOUD_NAME,API_KEY,API_SECRET);
            cloudinary = new Cloudinary(account);
        
        }

        public void InicializarValores()
        {
            CLOUD_NAME = config.GetValue<string>("Cloudinary:CLOUD_NAME");
            API_KEY = config.GetValue<string>("Cloudinary:API_KEY");
            API_SECRET = config.GetValue<string>("Cloudinary:API_SECRET");
        }

        public System.Uri subirImagen(Stream stream)
        {         
            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription("12332", stream)
                };

                var respuesta =  cloudinary.Upload(uploadParams);

                return respuesta.SecureUrl;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return null;
            }

        }

        public void borrarImagen(string idImagen)
        {           
            try
            {
                var deletionParams = new DeletionParams(idImagen)
                {
                    PublicId = idImagen
                };



                cloudinary.Destroy(deletionParams);


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }
    }
    }
