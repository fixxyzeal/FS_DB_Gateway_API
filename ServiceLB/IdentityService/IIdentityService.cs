using BO.ViewModels;
using System.Threading.Tasks;

namespace ServiceLB.IdentityService
{
    public interface IIdentityService
    {
        Task<AuthResultViewModel> Identity(AuthRequestModel auth);
    }
}