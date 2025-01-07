using System;
using System.Threading.Tasks;

namespace UniAttend.Core.Interfaces.Services
{
    public interface IExceptionHandler
    {
        Task<string> GetUserFriendlyMessageAsync(Exception exception);
        Task HandleExceptionAsync(Exception exception);
    }
}