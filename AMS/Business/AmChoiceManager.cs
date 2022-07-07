using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AmChoiceManager
    {
        public AmChoiceDAO AmChoiceDAO { get; set; }

        public AmChoiceManager()
        {
            AmChoiceDAO = new AmChoiceDAO();
        }

        public IList<AmChoice> FindByCode(string code)
        {
            List<AmChoice> result = new List<AmChoice>();
            result = this.AmChoiceDAO.FindByCode(code);
            return result;
        }

        public AmChoice FindByCodeValue(string code, string value)
        {
            return this.AmChoiceDAO.FindByCodeValue(code, value);
        }

        public List<AmChoice> FindCodeList()
        {
            return this.AmChoiceDAO.FindCodeList();
        }

        public void Insert(AmChoice entity)
        {
            this.AmChoiceDAO.Insert(entity);
        }

        public void Delete(string choiceCode, string choiceValue)
        {
            this.AmChoiceDAO.Delete(choiceCode, choiceValue);
        }

        public IList<AmChoice> FindByCodeAll(string code)
        {
            List<AmChoice> result = new List<AmChoice>();
            result = this.AmChoiceDAO.FindByCodeAll(code);
            return result;
        }

        public IList<AmChoice> FindByCodeSpecial(string code, string actValue)
        {
            List<AmChoice> result = new List<AmChoice>();
            result = this.AmChoiceDAO.FindByCode(code);
            return result;
        }

        public List<AmChoice> FindByCodeSpecial(string code, List<string> selections)
        {
            return this.AmChoiceDAO.FindByCodeSpecial(code, selections);
        }

        public AmChoice FindByCodeName(string code, string name)
        {
            return this.AmChoiceDAO.FindByCodeName(code, name);
        }
    }
}
