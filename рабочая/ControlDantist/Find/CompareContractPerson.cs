using System;
using System.Collections.Generic;
using ControlDantist.Find;
using System.Linq;
using System.Text;

namespace ControlDantist.Find
{
    public static class CompareContractPerson
    {
        //ListFindPersonNumContractItem
        public static List<FindPersonNumContractItem> Compare(List<FindPersonNumContractItem> listContract)
        {
            // Список для хранения результат фильтрации.
            List<FindPersonNumContractItem> list = new List<FindPersonNumContractItem>();

            // Сгруппируем поступивший список по ФИО и номеру договора и номеру акта.
            var groupCont = listContract.GroupBy(w => new { w.Фамилия, w.Имя, w.Отчество, w.НомерДоговора, w.НомерАкта });
            // Пройдемся по группе.
            foreach (var itms in groupCont)
            {
                // Если в группе больше одной записи.
                if (itms.Count() > 1)
                {
                    // Отфильтруем записанные в таблицу Add так как запись в таблице Add имеет приоритет.
                    var contractsAdd = itms.Where(w => w.Год == 2019);

                    // Если у льготника есть договора в таблице Add.
                    if (contractsAdd.Count() > 0)
                    {
                        if (contractsAdd.Count() >= 1)
                        {
                            // Отсортируем по возрастанию id договора (IdContract) и запишем первую запись.
                            // То есть мы пишем последную запись из БД так как она более актуальная.
                            foreach (var it in contractsAdd.OrderByDescending(x => x.id_договор).Take(1))
                            {
                                // Запишем первую запись из группы в списое а остальные проигнорируем.
                                list.Add(it);
                                continue;
                            }

                            continue;
                        }
                    }
                    else
                    {
                        // ЕСли записей с пометкой 2019 год нет. То есть нет заисей в NameTableAdd.
                        var contracts = itms.Where(w => w.Год != 2019);

                        if (contracts.Count() >= 1)
                        {
                            foreach (var it in contracts.OrderByDescending(x => x.id_договор).Take(1))
                            {
                                list.Add(it);
                                continue;
                            }

                            continue;
                        }
                    }
                }
                else
                {
                    // Если в группе по одной записи.
                    // Отфильтруем записанные в таблицу Add.
                    var contractsAdd = itms.Where(w => w.Год == 2019);

                    if (contractsAdd.Count() > 0)
                    {
                        foreach (var it in contractsAdd.OrderByDescending(x => x.id_договор).Take(1))
                        {
                            // Запишем первую запись из группы в списое а остальные проигнорируем.
                            list.Add(it);
                            continue;
                        }

                        continue;
                    }

                    // Отфильтруем записанные в БД.
                    var contracts = itms.Where(w => w.Год != 2019);

                    if (contracts.Count() > 0)
                    {
                        foreach (var it in contracts.OrderByDescending(x => x.id_договор).Take(1))
                        {
                            list.Add(it);
                            continue;
                        }

                        continue;
                    }
                }
            }

         return list;

        }
    }
}
