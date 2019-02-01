using Common.Interfaces;
using LibraryAmoCRM.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Common.BusinessLogicHelpers.Crm.Actions
{
    public abstract class ActionsBase
    {
        protected ILogger logger;
        protected IDataManager crm;

        public ActionsBase(IDataManager amoManager, ILogger logger)
        {
            this.logger = logger;
            this.crm = amoManager;
        }
    }
}
