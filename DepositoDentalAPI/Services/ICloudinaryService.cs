namespace DepositoDentalAPI.Services
{
    public interface ICloudinaryService
    {
        public System.Uri subirImagen(Stream stream);

        public void borrarImagen(string idImagen);
    }
}
