using Library1C;
using LibraryAmoCRM;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiBusinessLogic.Interfaces.Crm;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiBusinessLogic.Infrastructure.CrmDoEventActions
{
    public abstract class DoCrmActionBase : ICrmAction
    {
        protected DataManager amoManager;
        protected UnitOfWork database;

        public DoCrmActionBase( DataManager amoManager, UnitOfWork database )
        {
            this.amoManager = amoManager; this.database = database;
        }

        public virtual void DoAction(object sender, CrmEvent e)
        {
            throw new NotImplementedException();
        }
    }
}
