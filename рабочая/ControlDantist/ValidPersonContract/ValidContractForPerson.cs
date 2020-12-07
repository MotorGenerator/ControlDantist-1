using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Repository;
using System.Data;
using System.Data.SqlClient;
using ControlDantist.Classes;
using System.Threading;

namespace ControlDantist.ValidPersonContract
{
    /// <summary>
    /// Проверить договор для текущего льготника.
    /// </summary>
    public class ValidContractForPerson
    {
        private string famili;
        private string name;
        private string surname;
        private DateTime dateBirth;

        // Переменная для хранения id льготника.
        private int idPerson = 0;

        private UnitDate unitDate;

        private SqlConnection con;

        private SqlTransaction transact;

        // Объект для синхронизации.
        private static object obj = new object();

        //хранит данные для текущего льготника
        private PrintContractsValidate contr;

        private string numContract = string.Empty;

        // Мьютекс.
        static Mutex mutexObj = new Mutex();

        List<ValidItemsContract> listContracts;// = new List<ValidItemsContract>();

        /// <summary>
        /// Список договоров.
        /// </summary>
        //public List<ValidItemsContract> listContracts { get; set; }

        public ValidContractForPerson(string famili, string name, string surname, DateTime dateBirth)
        {
            this.famili = famili;
            this.name = name;
            this.surname = surname;
            this.dateBirth = dateBirth;

            contr = new PrintContractsValidate();

            contr.listContracts = new List<ValidItemsContract>();

            this.listContracts = new List<ValidItemsContract>();
        }

        /// <summary>
        /// Получаем номер договора.
        /// </summary>
        /// <param name="numContract"></param>
        public void SetNumContract(string numContract)
        {
            if (numContract != null)
            {
                this.numContract = numContract;
            }
            else
            {
                throw new Exception("Отсутствует номер договора");
            }
        }

        /// <summary>
        /// Устанавливаем SQl Транзакцию.
        /// </summary>
        /// <param name="transact"></param>
        public void SetSqlTransaction(SqlTransaction transact)
        {
            if (transact != null)
            {
                this.transact = transact;
            }
            else
            {
                throw new Exception("Отсутствует экземпляр транзакции");
            }

        }

        /// <summary>
        /// Получаем строку подключения.
        /// </summary>
        /// <param name="sqlConnection"></param>
        public void SetSqlConnection(SqlConnection sqlConnection)
        {
            if (sqlConnection != null)
            {
                this.con = sqlConnection;
            }
            else
            {
                throw new Exception("Не инициализировано подключение к БД");
            }
        }

        public PrintContractsValidate GetContract()
        {

            // Сарые методы.
            //ValidPrevioslyAct();
            //ValidPrevioslyNumContract();

            PersonParametr personParametr = new PersonParametr();
            personParametr.famili = this.famili;
            personParametr.name = this.name;
            personParametr.secondName = this.surname;
            personParametr.dateBirth = this.dateBirth;

            // Проверим записи в БД до 2019 года.
            IValidatePersonContract validatePersonContract = new ValidTo2019(this.famili, this.name, this.surname, this.dateBirth);
            string queryPC = validatePersonContract.Execute();

            StringParametr stringParametr = new StringParametr();
            stringParametr.Query = queryPC;

            BuildingSpike(stringParametr);

            // Проверим записи в 2019 году по таблицам NameTableAdd.
            IValidatePersonContract validatePersonContractTabAdd = new Valid2019TabAdd(this.famili, this.name, this.surname, this.dateBirth);
            string queryPc2019Add = validatePersonContractTabAdd.Execute();

            StringParametr stringParametr2019Add = new StringParametr();
            stringParametr2019Add.Query = queryPc2019Add;

            BuildingSpike(stringParametr2019Add);

            //// Проверим записи в 2019 году по БД.
            IValidatePersonContract validatePersonContract2019 = new Valid2019(this.famili, this.name, this.surname, this.dateBirth);
            string query2019 = validatePersonContract2019.Execute();

            StringParametr stringParametr2019 = new StringParametr();
            stringParametr2019.Query = query2019;

            BuildingSpike(stringParametr2019);

            // Проверим записи 2020 год и позже.
            IValidatePersonContract validatePersonContractAfter2019 = new ValidAfter2019(this.famili, this.name, this.surname, this.dateBirth);
            string queryAftar2019 = validatePersonContractAfter2019.Execute();

            StringParametr stringParametrAftar2019 = new StringParametr();
            stringParametrAftar2019.Query = queryAftar2019;

            BuildingSpike(stringParametrAftar2019);

            // Откажемся от потоков.
            //Thread thread = new Thread(new ParameterizedThreadStart(BuildingSpike));

            //Thread thread2019Add = new Thread(new ParameterizedThreadStart(BuildingSpike));

            //Thread thread2019 = new Thread(new ParameterizedThreadStart(BuildingSpike));

            //Thread threadAfter2019 = new Thread(new ParameterizedThreadStart(BuildingSpike));

            ///BuildingSpike(stringParametr);

            // Запустим потоки.
            //thread.Start(stringParametr);
            //thread2019Add.Start(stringParametr2019Add);
            //thread2019.Start(stringParametr2019);
            //threadAfter2019.Start(stringParametrAftar2019);

            // Запишем данные по договору.
            contr.НомерТекущийДоговор = this.numContract;

            StringBuilder buildFio = new StringBuilder();

            buildFio.Append(this.famili + " " + this.name + " " + this.surname);

            contr.ФИО_Номер_ТекущийДоговор = buildFio.ToString();

            var testList = this.listContracts.Count();

            // List<ValidItemsContract> listResultFind = 

                IWriteNumContract writeNumContract = new WriteNumContract(contr.listContracts);
                contr.СписокДоговоров = writeNumContract.Write();
            return contr;
        }



        #region Старый алгоритм не рабочий.
        ///// <summary>
        ///// Поиск ранее заключенных договоров которые имеют акт выполненных работ.
        ///// </summary>
        //private void ValidPrevioslyAct()
        //{
        //    string queryDubl = @"declare @FirstName nvarchar(150)
        //                        declare @Name nvarchar(150)
        //                        declare @Surname nvarchar(150)
        //                        declare @DateBirth date
        //                        declare @NumContract nchar(100)
        //               set @FirstName = '" + this.famili.ToString() + "' " +
        //                        " set @Name = '" + this.name.ToString() + "' " +
        //                        " set @Surname = '" + this.surname.ToString() + "' " +
        //                        " set @DateBirth = '" + Время.Дата(this.dateBirth.ToShortDateString().Trim()) + "' " +
        //                        @"select Договор.id_договор,НомерДоговора,ДатаДоговора as ДатаПодписания,Льготник.Фамилия,Льготник.Имя,Льготник.Отчество,ЛьготнаяКатегория.ЛьготнаяКатегория,
        //               SUM(УслугиПоДоговору.Сумма) as Сумма,Договор.ДатаЗаписиДоговора,Договор.НомерРеестра,Договор.НомерСчётФактрура
        //               ,Договор.logWrite,Договор.flag2019AddWrite from dbo.Договор
        //                      inner join Льготник
        //                        ON Договор.id_льготник = Льготник.id_льготник
        //                        inner join ЛьготнаяКатегория
        //                        ON Договор.id_льготнаяКатегория = ЛьготнаяКатегория.id_льготнойКатегории
        //                        inner join УслугиПоДоговору
        //                        ON УслугиПоДоговору.id_договор = Договор.id_договор
        //                        inner join АктВыполненныхРабот
        //                        ON АктВыполненныхРабот.id_договор = Договор.id_договор
        //                         where LOWER(RTRIM(LTRIM([Фамилия]))) = LOWER(LTRIM(RTRIM(@FirstName))) and LOWER(LTRIM(RTRIM(Имя))) = LOWER(LTRIM(RTRIM(@Name))) and((LOWER(LTRIM(RTRIM(Отчество))) = LOWER(LTRIM(RTRIM(@Surname)))) or Отчество is null)

        //                         and Льготник.ДатаРождения = @DateBirth
        //                        --and Договор.ФлагПроверки = 'False'
        //                        and YEAR(Договор.ДатаЗаписиДоговора) < 2019
        //                        and(ДатаАктаВыполненныхРабот is not null) and(YEAR(ДатаАктаВыполненныхРабот) <> 1900)
        //                        group by Договор.id_договор,НомерДоговора,Льготник.Фамилия,Льготник.Имя,Льготник.Отчество,ЛьготнаяКатегория.ЛьготнаяКатегория,
        //               Договор.ДатаЗаписиДоговора,Договор.НомерРеестра,Договор.НомерСчётФактрура ,АктВыполненныхРабот.НомерАкта
        //               ,АктВыполненныхРабот.ДатаПодписания,Договор.logWrite,Договор.flag2019AddWrite,ДатаДоговора
        //            union
        //            select MAX(id_договор) as id_договор,НомерДоговора,ДатаДоговора as ДатаПодписания,Фамилия,Имя,Отчество,ЛьготнаяКатегория,Сумма,ДатаЗаписиДоговора,НомерРеестра,
        //            НомерСчётФактрура,logWrite,flag2019AddWrite
        //            from(
        //            select ДоговорAdd.id_договор, НомерДоговора, ДатаДоговора, ЛьготникAdd.Фамилия, ЛьготникAdd.Имя, ЛьготникAdd.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория,
        //            SUM(УслугиПоДоговоруAdd.Сумма) as Сумма, ДоговорAdd.ДатаЗаписиДоговора, ДоговорAdd.НомерРеестра, ДоговорAdd.НомерСчётФактрура
        //            , ДоговорAdd.logWrite, ДоговорAdd.flag2019AddWrite from ЛьготникAdd
        //               inner join ДоговорAdd
        //               ON ДоговорAdd.id_льготник = ЛьготникAdd.id_льготник

        //               inner join ЛьготнаяКатегория
        //               ON ДоговорAdd.id_льготнаяКатегория = ЛьготнаяКатегория.id_льготнойКатегории

        //               inner join УслугиПоДоговоруAdd
        //               ON УслугиПоДоговоруAdd.id_договор = ДоговорAdd.id_договор

        //               left outer
        //                                                               join АктВыполненныхРаботAdd
        //                                                               ON АктВыполненныхРаботAdd.id_договор = ДоговорAdd.id_ТабДоговор
        //            where Фамилия = @FirstName and Имя = @Name and((Отчество = @Surname) or Отчество is null) and ЛьготникAdd.ДатаРождения = @DateBirth
        //            group by ДоговорAdd.id_договор, НомерДоговора, ЛьготникAdd.Фамилия, ЛьготникAdd.Имя, ЛьготникAdd.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория,
        //            ДоговорAdd.ДатаЗаписиДоговора, ДоговорAdd.НомерРеестра, ДоговорAdd.НомерСчётФактрура, ДатаПодписания, АктВыполненныхРаботAdd.НомерАкта, ДоговорAdd.logWrite, flag2019AddWrite, ДатаДоговора
        //            union
        //            select Договор.id_договор, НомерДоговора, ДатаДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория,
        //            SUM(УслугиПоДоговору.Сумма) as Сумма, Договор.ДатаЗаписиДоговора, Договор.НомерРеестра, Договор.НомерСчётФактрура
        //            , Договор.logWrite, Договор.flag2019AddWrite from Льготник
        //              inner join Договор
        //              ON Договор.id_льготник = Льготник.id_льготник

        //              inner join ЛьготнаяКатегория
        //              ON Договор.id_льготнаяКатегория = ЛьготнаяКатегория.id_льготнойКатегории

        //              inner join УслугиПоДоговору
        //              ON УслугиПоДоговору.id_договор = Договор.id_договор

        //              left outer
        //                                                         join АктВыполненныхРабот
        //                                                         ON АктВыполненныхРабот.id_договор = Договор.id_договор

        //                                                         inner join ProjectRegistrFiles
        //                                                         ON Договор.idFileRegistProgect = ProjectRegistrFiles.IdProjectRegistr
        //            where Фамилия = @FirstName and Имя = @Name and((Отчество = @Surname) or Отчество is null) and Льготник.ДатаРождения = @DateBirth
        //            group by Договор.id_договор, НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория,
        //            Договор.ДатаЗаписиДоговора, Договор.НомерРеестра, Договор.НомерСчётФактрура,
        //            ДатаПодписания, АктВыполненныхРабот.НомерАкта, Договор.logWrite, flag2019AddWrite, ДатаДоговора
        //            ) as Tab
        //            group by НомерДоговора,Фамилия,Имя,Отчество,ЛьготнаяКатегория,Сумма,ДатаЗаписиДоговора,НомерРеестра,
        //            НомерСчётФактрура,logWrite,flag2019AddWrite,ДатаДоговора";

        //    //string queryDubl = @"declare @F nvarchar(255)
        //    //                declare @I nvarchar(255)
        //    //                declare @O nvarchar(255)
        //    //                declare @dr date
        //    //                set @F = '" + this.famili.ToString() + "' " +
        //    //                " set @I = '" + this.name.ToString() + "' " +
        //    //                " set @O = '" + this.surname.ToString() + "' " +
        //    //                " set @dr = '" + Время.Дата(this.dateBirth.ToShortDateString().Trim()) + "' " +
        //    //                @"select НомерДоговора,ДатаДоговора as ДатаПодписания from dbo.Договор 
        //    //                inner join Льготник
        //    //                ON Договор.id_льготник = Льготник.id_льготник
        //    //                where [Фамилия] = @F and Имя = @I and Отчество = @O and ДатаРождения = @dr
        //    //                and YEAR(ДатаЗаписиДоговора) < 2019 and YEAR(ДатаЗаписиДоговора) >= 2018 and Договор.ФлагПроверки = 1
        //    //                union
        //    //                select НомерДоговора,ДатаДоговора as ДатаПодписания from dbo.ДоговорAdd 
        //    //                inner join ЛьготникAdd
        //    //                ON ДоговорAdd.id_льготник = ЛьготникAdd.id_льготник
        //    //                where [Фамилия] = @F and Имя = @I and Отчество = @O and ДатаРождения = @dr
        //    //                and ДоговорAdd.ФлагПроверки = 1
        //    //                UNION
        //    //                select НомерДоговора,ДатаДоговора as ДатаПодписания from dbo.Договор 
        //    //                inner join Льготник
        //    //                ON Договор.id_льготник = Льготник.id_льготник
        //    //                inner join ProjectRegistrFiles
        //    //                ON ProjectRegistrFiles.IdProjectRegistr = Договор.idFileRegistProgect
        //    //                where [Фамилия] = @F and Имя = @I and Отчество = @O and ДатаРождения = @dr
        //    //                and Договор.ФлагПроверки = 1
        //    //                and YEAR(ДатаЗаписиДоговора) >= 2020 ";



        //    DataTable tabContr = ТаблицаБД.GetTableSQL(queryDubl, "Дубли", con, transact);

        //    // Список с номерами ранее заключонных договоров для текущего льготника (как в текущей поликлиннике так и в других поликлинниках)
        //    StringBuilder listNumDog = new StringBuilder();

        //    foreach (DataRow row in tabContr.Rows)
        //    {

        //        if (DBNull.Value != row["НомерДоговора"])
        //        {

        //            if(this.numContract.Trim() == row["НомерДоговора"].ToString().Trim())
        //            {
        //                // Перейдем к следующей итерации.
        //                continue;
        //            }

        //            listNumDog.Append('\n' + " " + row["НомерДоговора"].ToString().Trim());// + " от " + Convert.ToDateTime(row["ДатаЗаписиДоговора"]).ToShortDateString().Trim());

        //        }

        //            if (DBNull.Value != row["ДатаПодписания"])
        //            {
        //                if (Convert.ToDateTime(row["ДатаПодписания"]).ToShortDateString().Trim() != "01.01.1900".Trim())
        //                {
        //                    listNumDog.Append(" от - " + Convert.ToDateTime(row["ДатаПодписания"]).ToShortDateString() + "; ");
        //                }
        //                else
        //                {
        //                    listNumDog.Append(" - проект договора ");
        //                }
        //            }

        //    }



        //    contr.СписокДоговоров = listNumDog.ToString();
        //}
        #endregion

        #region Старый алгоритм не рабочий

        ///// <summary>
        ///// Проверка на ранее заключенные договора в других поликлинниках.
        ///// </summary>
        ///// <returns></returns>
        //private void ValidPrevioslyNumContract()//(string numContract)
        //{
 
        //    string query = @"declare @F nvarchar(255)
        //                                declare @I nvarchar(255)
        //                                declare @O nvarchar(255)
        //                                declare @dr date
        //                                set @F = '" + this.famili.ToString() + "' " +
        //                    " set @I = '" + this.name.ToString() + "' " +
        //                    " set @O = '" + this.surname.ToString() + "' " +
        //                    " set @dr = '" + Время.Дата(this.dateBirth.ToShortDateString().Trim()) + "' " +
        //                    @"select НомерДоговора,ДатаДоговора from dbo.Договор 
        //    inner join Льготник
        //    ON Договор.id_льготник = Льготник.id_льготник
        //    where [Фамилия] = @F and Имя = @I and Отчество = @O and ДатаРождения = @dr
        //    and YEAR(ДатаЗаписиДоговора) < 2019 and Договор.ФлагПроверки = 1
        //    union
        //    select НомерДоговора,ДатаДоговора from dbo.ДоговорAdd 
        //    inner join ЛьготникAdd
        //    ON ДоговорAdd.id_льготник = ЛьготникAdd.id_льготник
        //    where [Фамилия] = @F and Имя = @I and Отчество = @O and ДатаРождения = @dr
        //    and ДоговорAdd.ФлагПроверки = 1
        //    UNION
        //    select НомерДоговора,ДатаДоговора from dbo.Договор 
        //    inner join Льготник
        //    ON Договор.id_льготник = Льготник.id_льготник
        //    inner join ProjectRegistrFiles
        //    ON ProjectRegistrFiles.IdProjectRegistr = Договор.idFileRegistProgect
        //    where [Фамилия] = @F and Имя = @I and Отчество = @O and ДатаРождения = @dr
        //    and Договор.ФлагПроверки = 1
        //    and YEAR(ДатаЗаписиДоговора) >= 2020 ";

        //    DataTable tabUnload = ТаблицаБД.GetTableSQL(query, "Договор", con, transact);

        //    //Присвоим списку индикации ФИО льготника
        //    string ФИО = this.famili + " " + this.name + " " + this.surname;

        //    contr.ФИО_Номер_ТекущийДоговор = ФИО.Trim();

        //    contr.НомерТекущийДоговор = numContract.Trim();

        //    //Динамическая строка для хранения 
        //    StringBuilder builder = new StringBuilder();

        //    // Получим договоры которые уже есть у льготника
        //    foreach (DataRow row in tabUnload.Rows)
        //    {
        //        //получим номер договора
        //        string номДог = row["НомерДоговора"].ToString().Trim();
        //        string датаДог = Convert.ToDateTime(row["ДатаДоговора"]).ToShortDateString();

        //        string договор = номДог + " от " + датаДог + " ; ";

        //        //если договор не подписан то и дату не выводим
        //        if (датаДог.Trim() == "01.01.1900")
        //        {
        //            договор = номДог + " ;";
        //        }

        //        builder.Append(договор);
        //    }

        //    //если договоров нет
        //    if (builder.Length == 0)
        //    {
        //        contr.НомераДоговоров = "нет";
        //    }
        //    else
        //    {
        //        contr.НомераДоговоров = "заключённые договора - " + builder.ToString().Trim();
        //    }
        //}

#endregion

        /// <summary>
        /// Построение списка письма.
        /// </summary>
        /// <param name="queryDubl"></param>
        private void BuildingSpike(object objParam)
        {

            StringParametr stringParametr = (StringParametr)objParam;

            mutexObj.WaitOne();

            //lock (listContracts)
            //{
            DataTable tabContr = ТаблицаБД.GetTableSQL(stringParametr.Query, "Дубли");//, con, transact);

                // Список с номерами ранее заключонных договоров для текущего льготника (как в текущей поликлиннике так и в других поликлинниках)
                StringBuilder listNumDog = new StringBuilder();

                foreach (DataRow row in tabContr.Rows)
                {
                    // Данные по текущему договору в таблице.
                    ValidItemsContract validItemsContract = new ValidItemsContract();

                    if (DBNull.Value != row["НомерДоговора"])
                    {

                        if (this.numContract.Trim() == row["НомерДоговора"].ToString().Trim())
                        {
                            // Перейдем к следующей итерации.
                            continue;
                        }

                        listNumDog.Append('\n' + " " + row["НомерДоговора"].ToString().Trim());// + " от " + Convert.ToDateTime(row["ДатаЗаписиДоговора"]).ToShortDateString().Trim());

                        validItemsContract.IdContract = Convert.ToInt32(row["id_договор"]);

                        // Номер договора
                        validItemsContract.NumContract = row["НомерДоговора"].ToString().Trim();

                        validItemsContract.flag2019Add = Convert.ToBoolean(row["flag2019AddWrite"]);

                        validItemsContract.CurrentNumContract = this.numContract;

                        // Запишем состояние договора, анулирован или нет.
                        validItemsContract.flagАнулирован = Convert.ToBoolean(row["flagАнулирован"]);

                    //flagАнулирован

                    }

                    if (DBNull.Value != row["ДатаПодписания"])
                    {
                        if (Convert.ToDateTime(row["ДатаПодписания"]).ToShortDateString().Trim() != "01.01.1900".Trim())
                        {
                            listNumDog.Append(" от - " + Convert.ToDateTime(row["ДатаПодписания"]).ToShortDateString() + "; ");

                            // Дата подписания договора.
                            validItemsContract.DateContract = Convert.ToDateTime(row["ДатаПодписания"]).ToShortDateString();
                        }
                        else
                        {
                            listNumDog.Append(" - проект договора ");
                        }
                    }

                    if (validItemsContract.flagАнулирован == true)
                    {
                        listNumDog.Append(" - договор анулирован ");
                    }
                

                this.contr.listContracts.Add(validItemsContract);

                }

                //если договоров нет
                if (listNumDog.Length == 0)
                {
                    contr.НомераДоговоров = "нет";
                }
                else
                {
                    contr.НомераДоговоров = "заключённые договора - " + listNumDog.ToString().Trim();
                }

            mutexObj.ReleaseMutex();
            //}
        }

    }
}
