using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class ListNumberProjectRepository : IRepository<ListNumbersProgectsReestr>
    {
        private DataClasses1DataContext dc;
        public ListNumberProjectRepository(DataClasses1DataContext dc)
        {
            this.dc = dc;
        }

        public void Delete(int id)
        {
            
        }

        public void DeleteAll(IEnumerable<ListNumbersProgectsReestr> items)
        {
            dc.ListNumbersProgectsReestr.DeleteAllOnSubmit(items);

            dc.SubmitChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Insert(ListNumbersProgectsReestr item)
        {
            //// Установим уровни изоляции транзакций.
            //var option = new System.Transactions.TransactionOptions();
            //option.IsolationLevel = System.Transactions.IsolationLevel.Serializable;

            //// Добавим льготника и адрес в БД.
            //// Внесём данные в таблицу в едино транзакции.
            //using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, option))
            //{
            //    try
            //    {
                    dc.ListNumbersProgectsReestr.InsertOnSubmit(item);

                    dc.SubmitChanges();

            //        //scope.Complete();
            //    }
            //    catch
            //    {
            //        throw new Exception("Запись файла реестра проектов договоров не произведена");
            //    }
            //}
        }

        public IEnumerable<ListNumbersProgectsReestr> Select(int id)
        {
            return dc.ListNumbersProgectsReestr?.Where(w => w.IdProjectRegistrFiles == id);
        }

       

        public void Update(ListNumbersProgectsReestr item)
        {
            throw new NotImplementedException();
        }
    }
}
