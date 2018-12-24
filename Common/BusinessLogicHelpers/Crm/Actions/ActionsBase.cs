using Common.Interfaces;
using LibraryAmoCRM.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Common.BusinessLogicHelpers.Crm.Actions
{
    public abstract class ActionsBase
    {
        protected ILoggerService logger;
        protected IDataManager crm;

        public ActionsBase(IDataManager amoManager, ILoggerService logger)
        {
            this.logger = logger;
            this.crm = amoManager;
        }
    }
}
