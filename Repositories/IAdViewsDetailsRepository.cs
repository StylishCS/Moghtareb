using Moghtareb.Models;

namespace Moghtareb.Repositories
{
    public interface IAdViewsDetailsRepository
    {
        void LogView(AdUserView adUserView);
        void LogPhoneView(AdUserPhoneView adUserPhoneView);
        void LogWhatsappView(AdUserWhatsappView adUserWhatsappView);
        List<AdUserView> GetViews();
        List<AdUserPhoneView> GetPhoneViews();
        List<AdUserWhatsappView> GetWhatsappViews();
        void Save();
    }
}