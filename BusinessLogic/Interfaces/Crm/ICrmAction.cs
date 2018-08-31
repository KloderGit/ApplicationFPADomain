using System;
using System.Collections.Generic;
using System.Text;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiBusinessLogic.Interfaces.Crm
{
    public interface ICrmAction
    {
        void DoAction(object sender, CrmEvent e);
    }
}
