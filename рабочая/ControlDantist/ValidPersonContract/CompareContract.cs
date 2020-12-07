using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ValidPersonContract
{
    public static class CompareContract 
    {
        public static List<ValidItemsContract> Compare(List<ValidItemsContract> listContract)
        {

            List<ValidItemsContract> list = new List<ValidItemsContract>();

            // Сгруппируем список по номеру договора и номерам ранее заключенных договоров.
            //var groupCont3 = listContract.GroupBy(w => new { w.CurrentNumContract, w.NumContract, w.flag2019Add });

            //var iCount = groupCont3.Count();

            var groupCont = listContract.GroupBy(w => new { w.CurrentNumContract, w.NumContract});

            // Пройдемся по группе.
            foreach (var itms in groupCont)
            {
                // Если в группе больше одной записи.
                if (itms.Count() > 1)
                {

                    // Отфильтруем записанные в таблицу Add.
                    var contractsAdd = itms.Where(w => w.flag2019Add == true);

                    // Если у льготника есть договора в таблице Add.
                    if (contractsAdd.Count() > 0)
                    {
                        if (contractsAdd.Count() > 1)
                        {
                            foreach (var it in contractsAdd.OrderByDescending(x => x.IdContract).Take(1))
                            {
                                // Запишем первую запись из группы в списое а остальные проигнорируем.
                                list.Add(it);
                                continue;
                            }

                            continue;
                        }

                        // Если у льготника есть один догововр с пометкой flagAdd2019.
                        if (contractsAdd.Count() == 1)
                        {
                            foreach (var it in contractsAdd.OrderByDescending(x => x.IdContract).Take(1))
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
                        // Отфильтруем записанные в БД.
                        var contracts = itms.Where(w => w.flag2019Add == false);

                        if (contracts.Count() > 1)
                        {
                            foreach (var it in contracts.OrderByDescending(x => x.IdContract).Take(1))
                            {
                                list.Add(it);
                                continue;
                            }

                            continue;
                        }

                        if (contracts.Count() == 1)
                        {
                            foreach (var it in contracts.OrderByDescending(x => x.IdContract).Take(1))
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
                    // Отфильтруем записанные в таблицу Add.
                    var contractsAdd = itms.Where(w => w.flag2019Add == true);

                    if (contractsAdd.Count() > 0)
                    {
                        foreach (var it in contractsAdd.OrderByDescending(x => x.IdContract).Take(1))
                        {
                            // Запишем первую запись из группы в списое а остальные проигнорируем.
                            list.Add(it);
                            continue;
                        }

                        continue;
                    }

                    // Отфильтруем записанные в БД.
                    var contracts = itms.Where(w => w.flag2019Add == false);

                    if (contracts.Count() > 0)
                    {
                        foreach (var it in contracts.OrderByDescending(x => x.IdContract).Take(1))
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
