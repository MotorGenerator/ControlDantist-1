using System;
using System.Collections.Generic;
using System.Text;
using ControlDantist.Classes;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;

using DantistLibrary;
using ControlDantist.WriteClassDB;
using ControlDantist.Repository;

namespace ControlDantist.Classes
{
    class WriteBD
    {
        //Хранит выгруженный реестр
        private List<Unload> list;

        private string льготнаяКатегория = string.Empty;
        private string документ = string.Empty;
        private string районОбласти = string.Empty;
        private string населённыйПункт = string.Empty;
        private string поликлинника = string.Empty;
        private string иннПоликлинники = string.Empty;
        private string главБух = string.Empty;
        private DataTable t;

        public IStringQuery queryWrite { get; set; }

        //указывает, что договор с таким номером уже записан в БД
        private bool flagDog = false;

        public UnitDate UnitDate { get; set; }

        public WriteBD(List<Unload> unload)
        {
            list = unload;
        }

        /// <summary>
        /// ID файла реестра проектов договоров.
        /// </summary>
        public int IdFIleRegistrProject { get; set; }

        public bool FlagAddDate { get; set; }

        /// <summary>
        /// Записывает реестр в базу данных
        /// </summary>
        public string Write()
        {

            //Установим русскую культуру
            CultureInfo newCInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            newCInfo.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = newCInfo;

            //Так как во время записи реестра в БД возхникает ситуация при которой 
            //льготник ещё не записан, а данные по поликлиннике уже в БД существуют
            //записывать будем в разных транзакциях(Решить тоже самое можно было с помощью TSQL)
            int iTest = list.Count;

            // Доработанный алгоритм записи льготников и проектов договоров в базу данных.

            // Строка для хранения SQL запроса к БД на запись проектов договоров.
            StringBuilder buildQuery = new StringBuilder();

            // Счётчик договоров.
            int iCountContract = 0;

            //Счётчик циклов
            int iCount = 0;

            int iCount2 = 0;

            int iCount33 = 0;

            int iCount4 = 0;


            foreach (Unload unload in list)
            {

                //IЛьготник personFio = new FindPerson();

                // // Загрузим данные по лготнику.
                // DataTable tabЛьгот = unload.Льготник;

                // // Данные по льготнику.
                // personFio.Famili = tabЛьгот.Rows[0]["Фамилия"].ToString().Trim();
                // personFio.Name = tabЛьгот.Rows[0]["Имя"].ToString().Trim();
                // personFio.SecondName = tabЛьгот.Rows[0]["Отчество"].ToString().Trim();
                // personFio.DateBirtch = Время.Дата(Convert.ToDateTime(tabЛьгот.Rows[0]["ДатаРождения"]).ToShortDateString());

                // // Наименование населенного пункта.
                 ISity sity = new NameSity();

                 DataTable tabSity = unload.НаселённыйПункт;

                // if(personFio.Famili == "КУРАЕВ")
                // {
                //     string iTest3 = "";
                // }


                if (tabSity.Rows.Count == 0)
                {
                    sity.NameTown = "ЗАТО Светлый";
                }
                else
                {
                    // Получим наименование населенного пункта в котором проживает льготник.
                    sity.NameTown = tabSity.Rows[0]["Наименование"].ToString().Trim();
                }

                // // Передадим данные по льготнику.
                // queryWrite.PersonFio = personFio;

                // // Населенный пункт.
                // queryWrite.NameSity = sity;

                UnitDate unitDate = new UnitDate();

                // Льготниая категория.
                queryWrite.ЛьготнаяКатегория = unload.ЛьготнаяКатегория.Trim();

                // Получим льготную категорию.
                Repository.ЛьготнаяКатегория льготнаяКатегория = unitDate.ЛьготнаяКатегорияRepository.GetЛьготнаяКатегория(unload.ЛьготнаяКатегория.Trim());

                // Тип документа.
                queryWrite.ТипДокумента = unload.ТипДокумента.Rows[0][1].ToString();

                // Порочитаем из файла выгрузки все данные по льготнику.
                DataRow rw_Льготник = unload.Льготник.Rows[0];

                ЛьготникAdd personFull = new ЛьготникAdd();

                personFull.Фамилия = rw_Льготник["Фамилия"].ToString().Trim();
                personFull.Имя = rw_Льготник["Имя"].ToString().Trim();
                personFull.Отчество = rw_Льготник["Отчество"].ToString().Trim();
                //personFull.DateBirtch = " Convert(datetime,'" + Время.Дата(Convert.ToDateTime(rw_Льготник["ДатаРождения"]).ToShortDateString().Trim()) + "',112)  ";
                personFull.ДатаРождения = Convert.ToDateTime(rw_Льготник["ДатаРождения"]);
                personFull.улица = rw_Льготник["улица"].ToString().Trim();
                personFull.НомерДома = rw_Льготник["НомерДома"].ToString().Trim();
                personFull.корпус = rw_Льготник["корпус"].ToString().Trim();
                personFull.НомерКвартиры = rw_Льготник["НомерКвартиры"].ToString().Trim();
                personFull.СерияПаспорта = rw_Льготник["СерияПаспорта"].ToString().Trim();
                personFull.НомерПаспорта = rw_Льготник["НомерПаспорта"].ToString().Trim();
                personFull.ДатаВыдачиПаспорта = Convert.ToDateTime(rw_Льготник["ДатаВыдачиПаспорта"]);
                personFull.КемВыданПаспорт = rw_Льготник["КемВыданПаспорт"].ToString().Trim();
                personFull.id_льготнойКатегории = льготнаяКатегория.id_льготнойКатегории;
                personFull.id_документ = 0;//                      ",@idДокумент_" + iCount + " " +
                personFull.СерияДокумента = rw_Льготник["СерияДокумента"].ToString().Trim();
                personFull.НомерДокумента = rw_Льготник["НомерДокумента"].ToString().Trim();
                personFull.ДатаВыдачиДокумента = Convert.ToDateTime(rw_Льготник["ДатаВыдачиДокумента"]);
                personFull.КемВыданДокумент = rw_Льготник["КемВыданДокумент"].ToString().Trim();
                personFull.id_область = 1;//id области у нас по умолчанию 
                personFull.id_район = Convert.ToInt16(rw_Льготник["id_район"]);

                 if((personFull.Фамилия.Trim().ToLower() == "Тюрина".ToLower().Trim()) && (personFull.Имя.Trim().ToLower() == "Галина".ToLower().Trim()) && (personFull.Отчество.Trim().ToLower() == "Викторовна".ToLower().Trim()))
                 {
                    DateTime dt = new DateTime(1942, 8, 2);

                    personFull.ДатаВыдачиПаспорта = dt;
                 }

                // Запишем id населенного пункта.
                var findSity = this.UnitDate.НаселенныйПунктRepository.FiltrНаселенныйПункт(sity.NameTown);

                if(findSity != null)
                {
                    personFull.id_насПункт = findSity.id_насПункт;
                }
                else
                {
                    НаселённыйПункт населённыйПункт = new НаселённыйПункт();
                    населённыйПункт.Наименование = sity.NameTown;

                    // Запишем по новой населенный пункт.
                    UnitDate.НаселенныйПунктRepository.Insert(населённыйПункт);

                    personFull.id_насПункт = населённыйПункт.id_насПункт;
                }

                // Проверим есть ли льготник с ФИО и датой рождения в БД.
                if(unitDate.ЛЬготникAddRepository.SelectPerson(personFull)!= null)
                {
                    // Обновим данные по льготнику.
                    unitDate.ЛЬготникAddRepository.Update(personFull);
                }
                else
                {
                    // Запишем нового льготника.
                    unitDate.ЛЬготникAddRepository.Insert(personFull);
                }
              

                // Запишем договор.
                //IContract contract = new Contract();

                DataRow rowC = unload.Договор.Rows[0];

                ДоговорAdd contract = new ДоговорAdd();

                // Получим данные по поликлиннике.
                DataRow rowHosp = unload.Поликлинника.Rows[0];

                // Прочитам данные по поликлиннике.
                int idHospital = unitDate.ПоликлинникаИННRepository.SelectИнн(rowHosp["ИНН"].ToString());

                contract.НомерДоговора = rowC["НомерДоговора"].ToString();
                contract.ДатаДоговора = Convert.ToDateTime("01.01.1900");
                contract.ДатаАктаВыполненныхРабот = Convert.ToDateTime("01.01.1900");
                contract.СуммаАктаВыполненныхРабот = 0.0m;   
                contract.id_льготнаяКатегория = льготнаяКатегория.id_льготнойКатегории;
                contract.id_льготник = personFull.id_льготник;
                contract.id_комитет = 1;
                contract.id_поликлинника = idHospital;
                contract.датаВозврата = null;
                contract.ДатаЗаписиДоговора = DateTime.Now.Date;
                contract.ДатаПроверки = null;
                contract.ДатаРеестра = null;
                contract.ДатаСчётФактура = null;
                contract.НомерРеестра = null;
                contract.НомерСчётФактрура = null;
                contract.Примечание = null;
                contract.СуммаАктаВыполненныхРабот = 0.0m;
                contract.ФлагАнулирован = false;
                contract.ФлагВозвратНаДоработку = false;
                contract.ФлагДопСоглашения = rowC["НомерДоговора"].ToString();
                contract.ФлагНаличияАкта = false;
                contract.ФлагНаличияДоговора = false;
                contract.ФлагПроверки = false;
                contract.флагСРН = null;
                contract.флагУслуги = null;
                contract.idFileRegistProgect = 0;
                contract.flagАнулирован = false;
                contract.flagОжиданиеПроверки = false;

                ////contract.Note = "";
                ////contract.FlagContract = false;
                ////contract.FalgAct = false;
                ////contract.numContract = rowC["НомерДоговора"].ToString();

                ////// Текущая дата.
                ////contract.DateWriteContract = Время.Дата(DateTime.Now.Date.ToShortDateString());

                //// Если договор стоит как прошедший проверку.
                // if (unload.FalgWrite == true)
                // {
                //     contract.FlagValidate = true;
                // }
                // else
                // {
                //    contract.FlagValidate = false;
                // }

                // Запишем ЛОГ кто записал.
                 contract.logWrite = MyAplicationIdentity.GetUses();

                //string iTestDate = contract.DateWriteContract;

                // queryWrite.contract = contract;

                // Запишем данные по договору.
                unitDate.ДоговорAddRepository.Insert(contract);


                 // Услуги по договору.
                 DataTable tabServices = unload.УслугиПоДоговору;

                // Переменная для хранения строки запроса на добавление услуг в контракт.
                 StringBuilder servicesInsert = new StringBuilder();

                 List<IServicesContract> listServicesContract = new List<IServicesContract>();

                // Сформируем запрос на добавление услуг.
                 foreach (DataRow row in tabServices.Rows)
                 {
                     УслугиПоДоговоруAdd services = new УслугиПоДоговоруAdd();
                     services.НаименованиеУслуги = row["НаименованиеУслуги"].ToString();
                     services.цена = Convert.ToDecimal(row["Цена"]);
                     services.Количество = Convert.ToInt32(row["Количество"]);
                    services.id_договор = contract.id_договор;
                     services.НомерПоПеречню = row["НомерПоПеречню"].ToString();
                     services.Сумма = Convert.ToDecimal(row["Сумма"]);
                     services.ТехЛист = Convert.ToInt16(row["ТехЛист"]);

                    unitDate.УслугиПоДоговоруAddRepository.Insert(services);
                 }


                // Получим данные о поликлиннике.
                DataRow rowHosp2  = unload.Поликлинника.Rows[0];

                IAddHospital ih = new InsertDateHospital();
                ih.НаименованиеПоликлинники = rowHosp2["НаименованиеПоликлинники"].ToString().Trim();
                ih.КодПоликлинники = rowHosp2["КодПоликлинники"].ToString().Trim();
                ih.ЮридическийАдрес = rowHosp2["ЮридическийАдрес"].ToString().Trim();
                ih.ФактическийАдрес = rowHosp2["ФактическийАдрес"].ToString().Trim();
                ih.id_главВрач = 1;
                ih.id_главБух =1;
                ih.СвидетельствоРегистрации = rowHosp2["СвидетельствоРегистрации"].ToString().Trim();
                ih.ИНН = rowHosp2["ИНН"].ToString().Trim();
                ih.КПП = rowHosp2["КПП"].ToString().Trim();
                ih.БИК = rowHosp2["БИК"].ToString().Trim();
                ih.НаименованиеБанка = rowHosp2["НаименованиеБанка"].ToString().Trim();
                ih.РасчётныйСчёт = rowHosp2["РасчётныйСчёт"].ToString().Trim();
                ih.ЛицевойСчёт = rowHosp2["ЛицевойСчёт"].ToString().Trim();
                ih.НомерЛицензии = rowHosp2["НомерЛицензии"].ToString().Trim();
                ih.ДатаРегистрацииЛицензии = Convert.ToDateTime(rowHosp2["ДатаРегистрацииЛицензии"]).ToShortDateString();
                ih.ОГРН = rowHosp2["ОГРН"].ToString().Trim();
                ih.СвидетельствоРегистрацииЕГРЮЛ = rowHosp2["СвидетельствоРегистрацииЕГРЮЛ"].ToString().Trim();
                ih.ОрганВыдавшийЛицензию = rowHosp2["ОрганВыдавшийЛицензию"].ToString().Trim();
                ih.Постановление = rowHosp2["Постановление"].ToString().Trim();
                ih.ОКПО = rowHosp2["ОКПО"].ToString().Trim();
                ih.ОКАТО = rowHosp2["ОКАТО"].ToString().Trim();
                ih.Flag = Convert.ToBoolean(rowHosp2["Flag"]);
                ih.НачальныйНомерДоговора = Convert.ToInt32(rowHosp2["НачальныйНомерДоговора"]);

                //// Получим данные о СНИЛС.
                //IHospital hospSnils = new SelectHjspital();
                
                //hospSnils.ИНН = rowHosp2["ИНН"].ToString().Trim();

                //queryWrite.hospInn = hospSnils;

                //// Получим данные по поликлиннике.
                //queryWrite.dataHospital = ih;

                //// Id файла проектов договоров.
                //queryWrite.IdFileRegistrProject = this.IdFIleRegistrProject;

                //// Запишем подзапрос на запись услуг по договору.
                ////queryWrite.QueryInsertServicesContract = servicesInsert.ToString();

                //queryWrite.GetServicesContract(listServicesContract);

                ////var testInsert = contract.DateWriteContract;

                //string iTest2 = "";

                // string strQuery = queryWrite.Query(iCount);

                // buildQuery.Append(strQuery);

                //iCount++;

                //string strQueryReceptionCOntract = queryWrite.QueryReception(iCount);

                //buildQuery.Append(strQueryReceptionCOntract);

                iCount++;

            }

            return buildQuery.ToString().Trim();
        }


        // }
    }
}
//}
