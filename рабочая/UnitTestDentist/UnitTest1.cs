using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlDantist.Repository;
using System.Collections.Generic;
using ControlDantist.BalanceContract;
using ControlDantist.Classes;
using System.Collections.Generic;
using System.Linq;
using ControlDantist.ValidateRegistrProject;
using DantistLibrary;
using ControlDantist.ValidPersonContract;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace UnitTestDentist
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Поиск льготников в БД.
        /// </summary>


        //[TestMethod]
        //public void DoShortText()
        //{
        //    // Организация 
        //    string input = "ветеран труда";

        //    СтрокаСимвол строкаСимвол = new СтрокаСимвол();
        //    var result = строкаСимвол.DoShortText(input);

        //    var italon = "ВТ";

        //    Assert.AreEqual(result, italon, "Преобразование строки в абравиатуру");
        //}

        //[TestMethod]
        //public void DoShortText2()
        //{
        //    // Организация 
        //    string input = "ветеран труда     саратовской области";

        //    СтрокаСимвол строкаСимвол = new СтрокаСимвол();
        //    var result = строкаСимвол.DoShortText(input);

        //    var italon = "ВТСО";

        //    Assert.AreEqual(result, italon, "Преобразование строки в абравиатуру");
        //}

        //[TestMethod]
        //public void DoShortText3()
        //{
        //    // Организация 
        //    string input = "";

        //    СтрокаСимвол строкаСимвол = new СтрокаСимвол();
        //    var result = строкаСимвол.DoShortText(input);

        //    var italon = "";

        //    Assert.AreEqual(result, italon, "Преобразование строки в абравиатуру");
        //}

        ///// <summary>
        ///// Работоспособность функции определяющий режим работы формы на добавление.
        ///// </summary>
        //[TestMethod]
        //public void AddYearLimitTest()
        //{
        //    // arrange
        //    var year = 2018;

        //    //act
        //    // Узнаем можем добавлять новый лимит или только редактирование.
        //    YearLimit yearLimit = new YearLimit(year);
        //    bool flarEdit = yearLimit.ValidateEdit();

        //    // accert
        //    Assert.AreEqual(flarEdit, false, "Добавляем лимит денежных средств");
        //}

        ///// <summary>
        ///// Работоспособность функции на редактирования лимита денежных средств.
        ///// </summary>
        //[TestMethod]
        //public void YearLimitTest()
        //{
        //    // arrange
        //    var year = 2019;

        //    //act
        //    // Узнаем можем добавлять новый лимит или только редактирование.
        //    YearLimit yearLimit = new YearLimit(year);
        //    bool flarEdit = yearLimit.ValidateEdit();

        //    // accert
        //    Assert.AreEqual(flarEdit, true, "Редактируем лимит денежных средств");
        //}

        //[TestMethod]
        //public void GetLimitTest()
        //{
        //    // Условия
        //    Year year1 = new Year();
        //    year1.Year1 = 2018;

        //    // Действие
        //    UnitDate unitDate = new UnitDate();
        //    Year selectYear = unitDate.YearRepository.Select(year1);
        //    if(selectYear == null)
        //    {
        //        Year nullYear = new Year();
        //        nullYear.intYear = 0;
        //        selectYear = nullYear;
        //    }
        //    var limitYear = unitDate.LimitMoneyRepository.SelectToYear(selectYear.intYear);

        //    string sum = limitYear.Sum(w => w.Limit).ToString();

        //    //string rez = "397577400,0000";
        //    string rez = "0";

        //    // Результат.
        //    Assert.AreEqual(sum, rez, "Сумма лимита на год");
        //}

        //[TestMethod]
        //public void ReceptionCOntractFormTest()
        //{
        //    // Репозиторий для доступа к БД.
        //    UnitDate unitDate = new UnitDate();

        //    // Откроем для теста окно сохранения проектов договоров.
        //    ControlDantist.ReceptionContractsForm receptionContractsForm = new ControlDantist.ReceptionContractsForm(unitDate);
        //    //receptionContractsForm.FlagWriteRegistrBase = false;
        //    System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.DialogResult.OK;

        //    bool flagTrue = false;

        //    if (dialogResult == System.Windows.Forms.DialogResult.OK)// && receptionContractsForm.FlagWriteRegistrBase == true)
        //    {
        //        flagTrue = true;

        //        Assert.IsTrue(flagTrue, "Загружаем реестр проекта договоров в БД.");
        //    }
        //    else
        //    {
        //        flagTrue = true;

        //        Assert.IsTrue(flagTrue, "Загружаем реестр проекта договоров для проверки");
        //    }
        //}

        //[TestMethod]
        //public void ПроверкаДаты()
        //{
        //    // arrange.
        //    DateTime dtInput = new DateTime(2019, 11, 19);

        //    bool flagValid = false;

        //    DateTime dtOut;

        //    // act.
        //    if (DateTime.TryParse(dtInput.ToShortDateString(), out dtOut) == true)
        //    {
        //        flagValid = true;
        //    }

        //    // assert.
        //    Assert.IsTrue(flagValid, "Проверка даты не прошла");
        //}

        //[TestMethod]
        //public void CreateItemRegistrProject()
        //{
        //    // arrange
        //    string numberLetter = "17";
        //    DateTime dateLetter = new DateTime(2019, 11, 28);
        //    int idHospital = 1;

        //    // act.
        //    UnitDate unitDate = new UnitDate();
        //    IQueryable<RegistrProject> registrProject = unitDate.ProjectRegistrFilesRepository.GetRegistr(numberLetter, dateLetter, idHospital);

        //    if (registrProject != null)
        //    {
        //        foreach (var itm in registrProject)
        //        {
        //            var asd = itm;
        //        }
        //        //this
        //    }
        //}

        //[TestMethod]
        //public void GetRegistrToClient()
        //{
        //    //arrange
        //    int idFile = 2;

        //    // act
        //    // Получим id записи.
        //    UnitDate unitDate = new UnitDate();

        //    Dictionary<string, Unload> unload = unitDate.ProjectRegistrFilesRepository.SelectFiles(idFile);

        //    Dictionary<string, Unload> unloadTest = new Dictionary<string, Unload>();

        //    // assert
        //    if (unload != null)
        //    {
        //        unloadTest = unload;

        //        Assert.AreEqual(unload, unloadTest, "Не найден реестр выгрузки проектов договоров.");
        //    }
        //    else
        //    {
        //        unloadTest = unload;

        //        Assert.AreEqual(unload, unloadTest, "Не найден реестр выгрузки проектов договоров.");
        //    }

        //}

        //[TestMethod]
        //public void GetДоговораTest()
        //{
        //    //arrange
        //    int idFile = 1;

        //    // act
        //    UnitDate unitDate = new UnitDate();
        //    var договорs = unitDate.ДоговорRepository.Select(idFile);



        //    if(договорs.Count() > 0)
        //    {
        //        int i = 1;
        //    }
        //    else
        //    {
        //        int i = 1;
        //    }

        //    // accert
        //}

        //[TestMethod]
        //public void РайонРепозиторийTest()
        //{

        //    // arrange.
        //    string nameRegion = "Саратовский";

        //    // act.
        //    РайонRepositroy районRepositroy = new РайонRepositroy(nameRegion);
        //}

        //[TestMethod]
        //public void RepositoryNumCOntractWereTest()
        //{
        //    // arrange.
        //    string numContract = "БакСП-1/3199";
        //    bool flag = false;

        //    // act
        //    UnitDate unitDate = new UnitDate();
        //    var numContracts = unitDate.RepositoryДоговорWhere.WhereNumContract(numContract);

        //    // accert.
        //    if (numContracts != null)
        //    {

        //        var test = numContracts.Count();

        //        var test2 = numContracts.ToList();

        //        foreach (var itm in numContracts)
        //        {
        //            var a = itm;
        //        }

        //        flag = true;

        //        Assert.IsTrue(flag, "Договор найден.");
        //    }
        //    else
        //    {
        //        flag = false;

        //        Assert.IsFalse(flag, "Договор не найден.");
        //    }

        //}

        //[TestMethod]
        //public void GetFioTest()
        //{
        //    // arrange
        //    // Получим ФИО льготника.
        //    string фамилия = "Иванов    Иван ";// Иванович";

        //    // Результат.
        //    string[] listResult = new string[3];

        //    listResult[0] = "Иванов";
        //    listResult[1] = "Иван";
        //    listResult[2] = "Иванович";


        //    // act.
        //    string[] arrayFio = фамилия.Split(' ');

        //    int iCount = 0;

        //    List<string> listFio = new List<string>();

        //    // Разобъем ФИО на 3 строки.
        //    foreach (string str in arrayFio)
        //    {
        //        if (str.Length > 0)
        //        {
        //            //arrayFio[iCount] = str;

        //            listFio.Add(str.Trim());

        //            iCount++;
        //        }
        //    }

        //    // Accert.
        //    //Assert.AreEqual(arrayFio, listResult, "Разборка строки с ФИО не прошла");
        //}

        //[TestMethod]
        //public void GetContractValideTesst()
        //{
        //    //// arrange.
        //    //PersonValidEsrn person = new PersonValidEsrn();
        //    //person.фамилия = "Коняхин";
        //    //person.имя = "Юрий";
        //    //person.отчество = "Ильич";
        //    //person.датаРождения = "1938-08-13";

        //    //string numContract = "МСЧС/41";

        //    PersonValidEsrn person = new PersonValidEsrn();
        //    person.фамилия = "Аккулова";
        //    person.имя = "Казира";
        //    //person.отчество = "Ильич";
        //    person.датаРождения = "1940-01-22";



        //    string numContract = "1 / 6557";

        //    string sConnect = "Data Source=10.159.102.68;Initial Catalog=Dentists;User ID=php;Password=1234";

        //    // act.
        //    //Узнаем содержатся ли ещё договора у текущего льготника
        //    using (SqlConnection con = new SqlConnection(sConnect))
        //    {
        //        con.Open();
        //        SqlTransaction transact = con.BeginTransaction();
        //        ValidContractForPerson validContract = new ValidContractForPerson(person.фамилия, person.имя, person.отчество.Do(x => x, ""), Convert.ToDateTime(person.датаРождения));
        //        validContract.SetSqlConnection(con);
        //        validContract.SetSqlTransaction(transact);
        //        validContract.SetNumContract(numContract);
        //        PrintContractsValidate договор = validContract.GetContract();
        //    }
        //}

        //[TestMethod]
        //public void TestConnet()
        //{
        //    UnitDate unitDate = new UnitDate();

        //    var iTest = unitDate.ПоликлинникаИННRepository.SelectAll();

        //    var count = iTest.Count();
        //}

        //[TestMethod]
        //public void TestCity()
        //{

        //    // Получим id записи.
        //    UnitDate unitDate = new UnitDate();

        //    //FiltrRepositoryДоговор filtrRepositoryДоговор = new FiltrRepositoryДоговор(unitDate);

        //    //var testContract = filtrRepositoryДоговор.Select(142332);

        //    //FiltrRepositoryДоговор filtrRepository = new FiltrRepositoryДоговор(unitDate.ДоговорRepository);

        //    //// ПОлучим текущий договор.
        //    //ControlDantist.Repository.Договор contract = filtrRepository.Select(142332);

        //    ControlDantist.Repository.Договор contract = unitDate.ДоговорRepository.FiltrДоговор(142332);

        //    if (contract != null)
        //    {
        //        // Получим id льготника.
        //        int idPerson = (int)contract.id_льготник;

        //        //var person = unitDate.ЛьготникRepository.Select(idPerson).FirstOrDefault();
        //        var person = unitDate.ЛьготникRepository.FiltrЛьготник(idPerson);

        //        if (person != null)
        //        {
        //            //Отобразим ФИО
        //            var a = person.Фамилия.Trim();
        //            var b = person.Имя.Trim();
        //            var c = person.Отчество.Trim();

        //            //отобразим серию и номер документа дающегно парво на льготу
        //            var a1 = person.СерияПаспорта.Trim();
        //            var a2 = person.НомерПаспорта.Trim();

        //            //отобразим серию и номер паспрота льготника
        //            var a3 = person.СерияДокумента.Trim();
        //            var a4 = person.НомерДокумента.Trim();


        //            //FiltrRepositoryГород filtrRepositoryThoun = new FiltrRepositoryГород(unitDate.НаселенныйПунктRepository);

        //            //var city = filtrRepositoryThoun.Select((int)person.id_насПункт);

        //            var city = unitDate.НаселенныйПунктRepository.FiltrНаселенныйПункт((int)person.id_насПункт);

        //            //отобразим адрес
        //            var a5 = "н.п. " + city.Наименование.Trim() + "  ул. " + person.улица.Trim();
        //            var a6 = "дом " + person.НомерДома;
        //            var a7 = "корп. " + person.корпус;
        //            var a8 = "кв. " + person.НомерКвартиры;

        //            var lk = unitDate.ЛьготнаяКатегорияRepository.Select((int)person.id_льготнойКатегории).FirstOrDefault();

        //            if (lk != null)
        //            {
        //                //отобразим льготную категорию
        //                var a9 = lk.ЛьготнаяКатегория1.Trim();
        //            }

        //        }
        //    }
        //}

        //[TestMethod]
        //public void ConfigLibraryTest()
        //{
        //    ConfigLibrary.Config config = new ConfigLibrary.Config();

        //    Dictionary<string, string> stringCOnnect = config.Select();

        //    foreach(var strCOnnect in stringCOnnect.Values)
        //    {
        //        var test = strCOnnect;
        //    }


        //}

        //[TestMethod]
        //public void GetRegistTest()
        //{
        //    // arrange 
        //    int IdFileRegistr = 186;

        //    // act.
        //    UnitDate unitDate = new UnitDate();

        //    ProjectRegistrFiles projectRegistrFiles = unitDate.ProjectRegistrFilesRepository.Select(IdFileRegistr).FirstOrDefault();
        //    projectRegistrFiles.flagValidateRegistr = true;

        //    // Accert.
        //}

        //[TestMethod]
        //public void DateWriteTest()
        //{
        //    var date = DateTime.Now.Date;

        //    var dateTest = DateTime.Today.Date;

        //    var dateWriteContract = ControlDantist.Classes.Время.Дата(DateTime.Now.Date.ToShortDateString());

        //    string iTest = "";
        //}

        //[TestMethod]
        //public void ЛьготникAddRepositoryTest()
        //{

        //    //arange
        //    ЛьготникAdd person = new ЛьготникAdd();
        //    person.Фамилия = "Гусев";
        //    person.Имя = "Владимир";
        //    //person.Отчество = "Федорович";

        //    DateTime dt = new DateTime(1942,8,2);

        //    person.ДатаРождения = dt;

        //    // act.
        //    DataClasses1DataContext dc = new DataClasses1DataContext();
        //    ЛЬготникAddRepository лЬготникAddRepository = new ЛЬготникAddRepository(dc);

        //    var result = лЬготникAddRepository.SelectPerson(person);
        //}

        //[TestMethod]
        //public void SelectИннTest()
        //{
        //// arrange.
        //string инн = "6455011350";

        //// act.
        //DataClasses1DataContext dc = new DataClasses1DataContext();

        //ПоликлинникаИННRepository поликлинникаИННRepository = new ПоликлинникаИННRepository(dc);
        //var id = поликлинникаИННRepository.SelectИнн(инн);

        //    //string sTest = "";
        //}

        //[TestMethod]
        //public void SelectГород()
        //{
        //    UnitDate unitDate = new UnitDate();

        //    // // Наименование населенного пункта.
        //    ISity sity = new NameSity();

        //    sity.NameTown = "";

        //    // Запишем id населенного пункта.
        //    var findSity = unitDate.НаселенныйПунктRepository.FiltrНаселенныйПункт(sity.NameTown);

        //    if (findSity != null)
        //    {
        //        var id_насПункт = findSity.id_насПункт;
        //    }
        //    else
        //    {
        //        НаселённыйПункт населённыйПункт = new НаселённыйПункт();
        //        населённыйПункт.Наименование = sity.NameTown;

        //        // Запишем по новой населенный пункт.
        //        unitDate.НаселенныйПунктRepository.Insert(населённыйПункт);


        //    }
        //}

        //private class MyPerson
        //{
        //    public string фамилия { get; set; }
        //    public string имя { get; set; }
        //    public string отчество { get; set; }
        //    public string датаРождения { get; set; }
        //}

        //[TestMethod]
        //public void LetterGarantirovannoeTest()
        //{
        //    // arrange
        //    List<PrintContractsValidate> listDoc = new List<PrintContractsValidate>();

        //    UnitDate unitDate = new UnitDate();


        //    using (SqlConnection con = new SqlConnection())
        //    {
        //        con.ConnectionString = "Data Source=10.159.102.21;Initial Catalog=Dentists;User ID=admin_dantist;Password=12A86Sql";
        //        con.Open();

        //        MyPerson person = new MyPerson();
        //        person.фамилия = "Клейменов";
        //        person.имя = "Виктор";
        //        person.отчество = "КОнстантинович";
        //        person.датаРождения = "23.06.1950";

        //        string numContract = "ЭСП-1/4025";

        //        SqlTransaction transact = con.BeginTransaction();

        //        ValidContractForPerson validContract = new ValidContractForPerson(person.фамилия, person.имя, person.отчество.Do(x => x, ""), Convert.ToDateTime(person.датаРождения));
        //        validContract.SetSqlConnection(con);
        //        validContract.SetSqlTransaction(transact);
        //        validContract.SetNumContract(numContract);
        //        PrintContractsValidate договор = validContract.GetContract();


        //        listDoc.Add(договор);

        //    }
        //}

        //[TestMethod]
        //public void GetDirectoriesTest()
        //{
        //    string path = @"F:\!\Зубы\ПРОВЕРКА";

        //    var iCountDir = Directory.GetDirectories(path).Length;

        //    // Пройдемся по вложенным директориям.
        //    foreach (var dir in Directory.GetDirectories(path))
        //    {
        //        var iTest = dir.Length;

        //        string[] files = Directory.GetFiles(dir);
        //    }
        //}

        //[TestMethod]
        //public void ValidRegisterTest()
        //{
        //    string dirName = @"F:\!\Зубы\2020 год\Проверка";

        //    //Переменная для хранения коллекции проектов договоров
        //    Dictionary<string, Unload> unload;

        //    foreach (var dir in Directory.GetDirectories(dirName))
        //    {
        //        // Проверим существует ли данная директория.
        //        if (Directory.Exists(dir))
        //        {
        //            string[] files = Directory.GetFiles(dir);

        //            // Пройдеся по файлам в выбранной директории.
        //            foreach (var fileName in files)
        //            {
        //                FileInfo fileInf = new FileInfo(fileName);

        //                string fileCopy = fileInf.Name;

        //                string fileFull = fileInf.FullName;

        //                using (FileStream fstream = File.Open(fileName, FileMode.Open))
        //                {
        //                    BinaryFormatter binaryFormatter = new BinaryFormatter();

        //                    try
        //                    {
        //                        // Проверим соответствует ли файл реестру.
        //                        //if ((Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream) is Dictionary<string, Unload>)
        //                        //{
        //                        // Получим из файла словарь с договорами.
        //                        unload = (Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream);
        //                        //}
        //                        //else
        //                        //{
        //                        //    continue;
        //                        //}
        //                    }
        //                    catch
        //                    {

        //                        using (TextWriter write = File.AppendText(@"F:\!\Зубы\2020 год\Ошибки\fileLog.txt"))
        //                        {
        //                            write.WriteLine(fileFull);
        //                        }

        //                        continue;

        //                    }
        //                }

        //            }

        //        }

        //    }

        //}


        //[TestMethod]
        //public void ValidateFiltrFalse()
        //{
        //    // arrange
        //    List<ValideContract> list = new List<ValideContract>();

        //    List<ValideContract> listTest3 = new List<ValideContract>();

        //    // Список для отображения на экране.
        //    List<ValideContract> listDisplay = new List<ValideContract>();

        //    ValideContract test1 = new ValideContract();

        //    test1.НомерДоговора = "10";

        //    test1.flag2019AddWrite = false;
        //    list.Add(test1);


        //    ValideContract test2 = new ValideContract();

        //    test2.НомерДоговора = "20";

        //    test2.flag2019AddWrite = false;
        //    list.Add(test2);


        //    ValideContract test3 = new ValideContract();

        //    test3.НомерДоговора = "20";

        //    test3.flag2019AddWrite = true;
        //    listTest3.Add(test3);

        //    // act.

        //    // Сгруппируем строки таблицы.
        //    var listG = list.GroupBy(w => w.НомерДоговора);

        //    foreach (var itms in listG)
        //    {
        //        if (itms.All(w => w.flag2019AddWrite == false) == true)
        //        {
        //            foreach (var itm in itms)
        //            {
        //                listDisplay.Add(itm);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var itm in itms)
        //            {
        //                if (itm.flag2019AddWrite == true)
        //                {
        //                    listDisplay.Add(itm);
        //                }
        //            }
        //        }
        //    }

        //    // assert
        //    Assert.AreEqual(test3, test3, "Тест не прошёл");
        //}



        //[TestMethod]
        //public void ValidCompareContract()
        //{
        //    // 1 flag2019Add - false, 2 - flag2019Add - true.

        //    /*

        //    // arrange.
        //    List<ValidItemsContract> list = new List<ValidItemsContract>();

        //    ValidItemsContract it = new ValidItemsContract();
        //    it.CurrentNumContract = "8/7184";

        //    it.IdContract = 124668;

        //    it.NumContract = "2-2/9182";
        //    it.flag2019Add = false;

        //    list.Add(it);

        //    ValidItemsContract it2 = new ValidItemsContract();
        //    it2.CurrentNumContract = "8/7184";

        //    it.IdContract = 3311;

        //    it2.NumContract = "2-2/9182";
        //    it2.flag2019Add = true;


        //    list.Add(it2);

        //    ValidItemsContract it3 = new ValidItemsContract();
        //    it3.CurrentNumContract = "8/7184";
        //    it3.IdContract = 3483;

        //    it3.NumContract = "2-2/9182";
        //    it3.flag2019Add = true;

        //    list.Add(it3);

        //    */

        //    //// arrange.
        //    //List<ValidItemsContract> list = new List<ValidItemsContract>();

        //    //ValidItemsContract it = new ValidItemsContract();
        //    //it.CurrentNumContract = "8/7184";

        //    //it.IdContract = 124668;

        //    //it.NumContract = "2-2/9182";
        //    //it.flag2019Add = true;

        //    //list.Add(it);

        //    //ValidItemsContract it2 = new ValidItemsContract();
        //    //it2.CurrentNumContract = "8/7184";

        //    //it.IdContract = 3311;

        //    //it2.NumContract = "2-2/9182";
        //    //it2.flag2019Add = false;


        //    //list.Add(it2);

        //    //ValidItemsContract it3 = new ValidItemsContract();
        //    //it3.CurrentNumContract = "8/7184";
        //    //it3.IdContract = 3483;

        //    //it3.NumContract = "2-2/9182";
        //    //it3.flag2019Add = false;

        //    //list.Add(it3);

        //    /*
        //    // arrange.
        //    List<ValidItemsContract> list = new List<ValidItemsContract>();

        //    ValidItemsContract it = new ValidItemsContract();
        //    it.CurrentNumContract = "8/7184";

        //    it.IdContract = 124668;

        //    it.NumContract = "2-2/9182";
        //    it.flag2019Add = true;

        //    list.Add(it);
        //    */

        //    /*
        //    // arrange.
        //    List<ValidItemsContract> list = new List<ValidItemsContract>();

        //    ValidItemsContract it = new ValidItemsContract();
        //    it.CurrentNumContract = "8/7184";

        //    it.IdContract = 124668;

        //    it.NumContract = "2-2/9182";
        //    it.flag2019Add = false;

        //    list.Add(it);
        //    */

        //    /*
        //    // arrange.
        //    List<ValidItemsContract> list = new List<ValidItemsContract>();

        //    ValidItemsContract it = new ValidItemsContract();
        //    it.CurrentNumContract = "8/7184";

        //    it.IdContract = 124668;

        //    it.NumContract = "2-2/9182";
        //    it.flag2019Add = true;

        //    list.Add(it);

        //    ValidItemsContract it2 = new ValidItemsContract();
        //    it2.CurrentNumContract = "8/7184";

        //    it.IdContract = 3311;

        //    it2.NumContract = "2-2/9183";
        //    it2.flag2019Add = false;


        //    list.Add(it2);

        //    ValidItemsContract it3 = new ValidItemsContract();
        //    it3.CurrentNumContract = "8/7184";
        //    it3.IdContract = 3483;

        //    it3.NumContract = "2-2/9184";
        //    it3.flag2019Add = false;

        //    list.Add(it3);


        //    // act.
        //    List<ValidItemsContract> listResult = CompareContract.Compare(list);

        //    // assert.
        //    ////List<ValidItemsContract> listRez = new List<ValidItemsContract>();

        //    ValidItemsContract validRez = listResult.First();

        //    ValidItemsContract rez = new ValidItemsContract();
        //    rez.CurrentNumContract = "8/7184";
        //    rez.IdContract = 3483;

        //    rez.NumContract = "2-2/9182";
        //    rez.flag2019Add = true;

        //    Assert.AreEqual< ValidItemsContract>(validRez, rez, "Тест не прошёл");
        //    */
        //}

        [TestMethod]
        public void TestWriteContract()
        {
            DateTime dr = new DateTime(1956, 1, 12);

            PrintContractsValidate contr = new PrintContractsValidate();// ("Иванов", "Иван", "Иванович", dr);

            contr.ФИО_Номер_ТекущийДоговор = "Иванов Иван Иванович";

            contr.НомерТекущийДоговор = "2-3/2802";

            contr.listContracts = new List<ValidItemsContract>();

            List<ValidItemsContract> list = new List<ValidItemsContract>();

            ValidItemsContract it = new ValidItemsContract();
            it.CurrentNumContract = "8/7184";
            it.DateContract = "17.08.2009";

            it.IdContract = 124668;

            it.NumContract = "2-2/9182";
            it.flag2019Add = true;

            list.Add(it);

            ValidItemsContract it2 = new ValidItemsContract();
            it2.CurrentNumContract = "8/7184";
            it2.DateContract = "01.01.1900";

            it.IdContract = 3311;

            it2.NumContract = "2-2/9183";
            it2.flag2019Add = false;

            list.Add(it2);

            ValidItemsContract it3 = new ValidItemsContract();
            it3.CurrentNumContract = "8/7184";
            it3.IdContract = 3483;
            it3.DateContract = "14.06.2020";

            it3.NumContract = "2-2/9184";
            it3.flag2019Add = false;

            list.Add(it3);

            contr.listContracts.AddRange(list);

            IWriteNumContract writeNumContract = new WriteNumContract(contr.listContracts);
            contr.НомераДоговоров = writeNumContract.Write();

            var test = "";

        }

        [TestMethod]
        public void TestFindContract()
        {
            List<ValideContract> list = new List<ValideContract>();

            //#region Работает
            ////ValideContract it1 = new ValideContract();

            ////it1.id_договор = "12490";
            ////it1.IdContract = 12490;
            ////it1.Фамилия = "Барышникова";
            ////it1.Имя = "Наталья";
            ////it1.Отчество = "Александровна";
            ////it1.НомерДоговора = "8/5917";
            ////it1.НомерАкта = "8/5917/5706";
            ////it1.Год = 2019;

            ////list.Add(it1);

            ////ValideContract it2 = new ValideContract();

            ////it2.id_договор = "120880";
            ////it2.IdContract = 120880;
            ////it2.Фамилия = "Барышникова";
            ////it2.Имя = "Наталья";
            ////it2.Отчество = "Александровна";
            ////it2.НомерДоговора = "8/5917";
            ////it2.НомерАкта = "8/5917/5706";
            ////it2.Год = 2019;

            ////list.Add(it2);

            //#endregion

            //#region Работает
            ////ValideContract it1 = new ValideContract();

            ////it1.id_договор = "12490";
            ////it1.IdContract = 12490;
            ////it1.Фамилия = "Барышникова";
            ////it1.Имя = "Наталья";
            ////it1.Отчество = "Александровна";
            ////it1.НомерДоговора = "8/5917";
            ////it1.НомерАкта = "8/5917/5706";
            ////it1.Год = 1;

            ////list.Add(it1);

            ////ValideContract it2 = new ValideContract();

            ////it2.id_договор = "120880";
            ////it2.IdContract = 120880;
            ////it2.Фамилия = "Барышникова";
            ////it2.Имя = "Наталья";
            ////it2.Отчество = "Александровна";
            ////it2.НомерДоговора = "8/5917";
            ////it2.НомерАкта = "8/5917/5707";
            ////it2.Год = 2019;

            ////list.Add(it2);

            //#endregion


            //ValideContract it1 = new ValideContract();

            //it1.id_договор = "12490";
            //it1.IdContract = 12490;
            //it1.Фамилия = "Барышникова";
            //it1.Имя = "Наталья";
            //it1.Отчество = "Александровна";
            //it1.НомерДоговора = "8/5917";
            //it1.НомерАкта = "8/5917/5706";
            //it1.Год = 2018;

            //list.Add(it1);

            //ValideContract it2 = new ValideContract();

            //it2.id_договор = "120880";
            //it2.IdContract = 120880;
            //it2.Фамилия = "Барышникова";
            //it2.Имя = "Наталья";
            //it2.Отчество = "Александровна";
            //it2.НомерДоговора = "8/5917";
            //it2.НомерАкта = "8/5917/5706";
            //it2.Год = 2018;

            //list.Add(it2);

            ValideContract it1 = new ValideContract();

            it1.id_договор = "19462";
            it1.IdContract = 19462;
            it1.Фамилия = "Щеглова";
            it1.Имя = "Любовь";
            it1.Отчество = "Петровна";
            it1.НомерДоговора = "1/9370";
            it1.НомерАкта = "1/9370/8501";
            it1.Год = 1;

            list.Add(it1);

            ValideContract it2 = new ValideContract();

            it2.id_договор = "641";
            it2.IdContract = 641;
            it2.Фамилия = "Старикова";
            it2.Имя = "Лидия";
            it2.Отчество = "Александровна";
            it2.НомерДоговора = "1/9370";
            it2.НомерАкта = "1/9370/8501";
            it2.Год = 2019;

            list.Add(it2);

            CompareContractForNumber.Compare(list);
        }

        [TestMethod]
       public void WriteToWordContract()
        {

            //Узнаем содержатся ли ещё договора у текущего льготника
            using (SqlConnection con = new SqlConnection("Data Source=10.159.102.21;Initial Catalog=Dentists;User ID=admin_dantist;Password=12A86Sql"))
            {
                con.Open();

                SqlTransaction transact = con.BeginTransaction();

                // Проверим есть ли у данного льготника ещё заключенные договора.
                ValidContractForPerson validContract = new ValidContractForPerson("Щеглова", "Любовь", "Петровна".Do(x => x, ""), Convert.ToDateTime("04.07.1951"));
                //validContract.listContracts = listContracts;
                validContract.SetSqlConnection(con);
                validContract.SetSqlTransaction(transact);
                validContract.SetNumContract("3/6334");
                PrintContractsValidate договор = validContract.GetContract();

            }
        }
    }
}
