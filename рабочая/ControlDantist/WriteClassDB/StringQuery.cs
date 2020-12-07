using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.WriteClassDB
{
    /// <summary>
    /// Класс формирования строки запроса для добавления льготников и договоров в БД.
    /// </summary>
    public class StringQuery : IStringQuery
    {
        public IЛьготник PersonFio { get; set; }
        public ISity NameSity { get; set; }
        public string ЛьготнаяКатегория { get; set; }
        public string ТипДокумента { get; set; }

        public IЛьготникFull pf { get; set; }

        public IContract contract {get; set;}

        /// <summary>
        /// id Файла проектов договоров.
        /// </summary>
        public int IdFileRegistrProject { get; set; }

        //public string QueryInsertServicesContract { get; set; }

        private IEnumerable<IServicesContract> list;

        public void GetServicesContract(IEnumerable<IServicesContract> listContract)
        {
            if (listContract == null)
            {
                list = new List<IServicesContract>();
            }
            else
            {
                list = listContract;
            }
        }

        private string QueryInsertServicesContract(string id_contract)
        {
            StringBuilder builder = new StringBuilder();

            foreach(IServicesContract item in list)
            {

                IQueryServices queryServices = new WriteServices(item);

                builder.Append(queryServices.Query(id_contract));
            }

            return builder.ToString();
        }


        private string QueryInsertServicesReceptionContract(string id_contract)
        {
            StringBuilder builder = new StringBuilder();

            foreach (IServicesContract item in list)
            {

                IQueryServices queryServices = new WriteServices(item);

                builder.Append(queryServices.QueryReceptionContract(id_contract));
            }

            return builder.ToString();
        }


        // ИНН поликлинники.
        public IHospital hospInn { get; set; }

        /// <summary>
        /// Данные по поликлиннике.
        /// </summary>
        public IAddHospital dataHospital { get; set; }

        public string Query(int count)
        {
            string query = " declare @idContract_" + count + " int  " + 
                            "declare @IdHosp_"+ count +" int " +
                            "select @IdHosp_" + count + " = MAX(id_поликлинника) from Поликлинника " +
                            "where ИНН = '" + hospInn.ИНН + "' " +
                            "if(@IdHosp_" + count + " = 0) " +
                            "begin " +
                                " INSERT INTO [Поликлинника] " +
                                           " ([НаименованиеПоликлинники] " +
                                           " ,[КодПоликлинники] " +
                                           " ,[ЮридическийАдрес] " +
                                           " ,[ФактическийАдрес] " +
                                           " ,[id_главВрач] " +
                                           " ,[id_главБух] " +
                                           " ,[СвидетельствоРегистрации] " +
                                           " ,[ИНН] " +
                                           " ,[КПП] " +
                                           " ,[БИК] " +
                                           " ,[НаименованиеБанка] " +
                                           " ,[РасчётныйСчёт] " +
                                           " ,[ЛицевойСчёт] " +
                                           ",[НомерЛицензии] " +
                                           ",[ДатаРегистрацииЛицензии] " +
                                           ",[ОГРН] " +
                                           ",[СвидетельствоРегистрацииЕГРЮЛ] " +
                                           ",[ОрганВыдавшийЛицензию] " +
                                           ",[Постановление] " +
                                           ",[ОКПО] " +
                                           ",[ОКАТО] " +
                                           ",[Flag] " +
                                           ",[НачальныйНомерДоговора]) " +
                                           "VALUES " +
                                           "('" + dataHospital.НаименованиеПоликлинники.ToString().Trim() + "' " +
                                           ",'" + dataHospital.КодПоликлинники.ToString().Trim() + "' " +
                                           ",'" + dataHospital.ЮридическийАдрес.ToString().Trim() + "' " +
                                           ",'" + dataHospital.ФактическийАдрес.ToString().Trim() + "' " +
                                           ",1 " +
                                           ",1 " +
                                            ",'" + dataHospital.СвидетельствоРегистрации.ToString().Trim() + "' " +
                                            ",'" + dataHospital.ИНН.ToString().Trim() + "' " +
                                           ",'" +  dataHospital.КПП.ToString().Trim() + "' " +
                                           ",'" +  dataHospital.БИК.ToString().Trim() + "' " +
                                          ",'" +   dataHospital.НаименованиеБанка.ToString().Trim() + "' " +
                                           ",'" +  dataHospital.РасчётныйСчёт.ToString().Trim() + "' " +
                                          ",'" +   dataHospital.ЛицевойСчёт.ToString().Trim() + "' " +
                                           ",'" +  dataHospital.НомерЛицензии.ToString().Trim() + "' " +
                                           ",'" +  dataHospital.ДатаРегистрацииЛицензии.ToString().Trim() + "' " +
                                           ",'" +  dataHospital.ОГРН.ToString().Trim() + "' " +
                                           ",'" +  dataHospital.СвидетельствоРегистрацииЕГРЮЛ.ToString().Trim() + "' " +
                                           ",'" +  dataHospital.ОрганВыдавшийЛицензию.ToString().Trim() + "' " +
                                           ",'" +  dataHospital.Постановление.ToString().Trim() + "' " +
                                           ",'" +  dataHospital.ОКПО.ToString().Trim() + "' " +
                                           ",'" +  dataHospital.ОКАТО.ToString().Trim() + "' " +
                                           ",'" +   dataHospital.Flag + "' " +
                                           "," +   dataHospital.НачальныйНомерДоговора + " ) " +
                                           " set @IdHosp_"+ count +" = @@IDENTITY " +
                            "end " +
                            "declare @id_насПункт_" + count + " int " +
                            "set @id_насПункт_" + count + " = 0 " +
                            "select @id_насПункт_" + count + " = id_насПункт from НаселённыйПункт " +
                            "where Наименование = '" + NameSity.NameTown + "'  " +
                             " declare @idЛК_" + count + "  int  " +
                        " select @idЛК_" + count + " = id_льготнойКатегории from ЛьготнаяКатегория " +
                        " where LOWER(RTRIM(LTRIM(ЛьготнаяКатегория))) = '" + ЛьготнаяКатегория.ToLower().Trim() + "'  " +
                            "if(select  COUNT(id_договор) from Договор " +
                            "inner join Льготник " +
                            "ON Льготник.id_льготник = Договор.id_льготник " +
                            "where LOWER(RTRIM(LTRIM(Фамилия))) = LOWER(RTRIM(LTRIM('" + PersonFio.Famili + "')))  " +
                            "and LOWER(RTRIM(LTRIM(Имя))) = LOWER(RTRIM(LTRIM('" + PersonFio.Name + "')))  " +
                            "and LOWER(RTRIM(LTRIM(Отчество))) = LOWER(RTRIM(LTRIM('" + PersonFio.SecondName + "')))  " +
                            "and ДатаРождения =  '" + PersonFio.DateBirtch + "' " +
                            "and ФлагПроверки = 'True') = 0 " +
                            " begin   " +
                        "if(@id_насПункт_" + count + ") = 0 " +
                        "begin " +
                            "INSERT INTO НаселённыйПункт(Наименование)  " +
                            "VALUES('" + NameSity.NameTown + "')  " +
                        "end " +
                        "declare @idДокумент_" + count + " int  " +
                        "select @idДокумент_" + count + " = id_документ from ТипДокумента " +
                        " where LOWER(RTRIM(LTRIM(НаименованиеТипаДокумента))) = '" + ТипДокумента.ToLower().Trim() + "' " +
                        "end " + // Только что добавил.
                        "if(select  COUNT(id_договор) from Договор " +
                            "inner join Льготник " +
                            "ON Льготник.id_льготник = Договор.id_льготник " +
                            "where LOWER(RTRIM(LTRIM(Фамилия))) = LOWER(RTRIM(LTRIM('" + PersonFio.Famili + "')))  " +
                            "and LOWER(RTRIM(LTRIM(Имя))) = LOWER(RTRIM(LTRIM('" + PersonFio.Name + "')))  " +
                            "and LOWER(RTRIM(LTRIM(Отчество))) = LOWER(RTRIM(LTRIM('" + PersonFio.SecondName + "')))  " +
                            "and ДатаРождения =  '" + PersonFio.DateBirtch + "' ) = 0 " +
                            "begin " +
                                " INSERT INTO Льготник ([Фамилия] ,[Имя] ,[Отчество] ,[ДатаРождения] ,[улица] ,[НомерДома] , " +
                                "[корпус] ,[НомерКвартиры] ,[СерияПаспорта] ,[НомерПаспорта] ,[ДатаВыдачиПаспорта] , " +
                                "[КемВыданПаспорт] ,[id_льготнойКатегории] ,[id_документ] ,[СерияДокумента] ,[НомерДокумента] , " +
                                "[ДатаВыдачиДокумента] ,[КемВыданДокумент] ,[id_область] ,[id_район] ,[id_насПункт] ,Снилс) " +
                                "VALUES ('" + pf.Famili + "' " +
                                ",'" + pf.Name + "' " +
                                ",'" + pf.SecondName + "' " +
                                ", '" + pf.DateBirtch + "' " +
                //", Convert(datetime,'19520324',112)  " + 
                                ",'" + pf.улица + "' " +
                                ",'" + pf.НомерДома + "' " +
                                ",'" + pf.корпус + "' " +
                                ",'" + pf.НомерКвартиры + "'  " +
                                ",'" + pf.СерияПаспорта + "' " +
                                ",'" + pf.НомерПаспорта + "' " +
                                ",'" + pf.ДатаВыдачиПаспорта + "' " +
                                ",'" + pf.КемВыданПаспорт + "'  " +
                                ",@idЛК_" + count + " " +
                                ",@idДокумент_" + count + " " +
                                ",'" + pf.СерияДокумента + "' " +
                                ",'" + pf.НомерДокумента + "' " +
                                ",'" + pf.ДатаВыдачиДокумента + "' ,'" + pf.КемВыданДокумент + "' ,1 , " + pf.id_район + "  " +
                                ",@id_насПункт_" + count + ",'')  " +
                                "INSERT INTO [Договор] ([НомерДоговора] ,[ДатаДоговора] ,[ДатаАктаВыполненныхРабот] " +
                                ",[СуммаАктаВыполненныхРабот] ,[id_льготнаяКатегория] ,[id_поликлинника] ,[Примечание] " +
                                ",[id_комитет] ,[ФлагНаличияДоговора] ,[ФлагНаличияАкта]  ,[id_льготник] ,[ФлагДопСоглашения] " +
                                ",[ДатаЗаписиДоговора] ,[ФлагПроверки] ,logWrite,flagОжиданиеПроверки, idFileRegistProgect,ФлагАнулирован,ФлагВозвратНаДоработку ) " +
                                "VALUES ('" + contract.numContract + "' , '" + contract.DateContract + "' , '" + contract.DataAct + "' , " + contract.SummAct + " " +
                                ",@idЛК_" + count + " ,@IdHosp_" + count + " ,'" + contract.Note + "' " +
                                "," + contract.IdConmmite + ",'False' ,'False' " +
                                ",@@IDENTITY,'" + contract.numContract + "' " +
                                ",'" + contract.DateWriteContract + "' ,'" + contract.FlagValidate + "' ,'" + contract.logWrite + "','false',"+ this.IdFileRegistrProject + ",'false','false' ) " +
                                //" declare @idContract_" + count + " int  " + 
                                " set @idContract_" + count + " = @@IDENTITY" + 
                                " " + QueryInsertServicesContract("@idContract_" + count + " " ) +  // Здесь услуги по договору.
                            "end " +
                            //"end " +
                            "else " +
                            "begin " +
                            "declare @id_person__" + count + " int " +
                            "select @id_person__" + count + " = Max(id_льготник) from Льготник " +
                            "where LOWER(RTRIM(LTRIM(Фамилия))) = LOWER(RTRIM(LTRIM('" + PersonFio.Famili + "')))  " +
                            "and LOWER(RTRIM(LTRIM(Имя))) = LOWER(RTRIM(LTRIM('" + PersonFio.Name + "')))  " +
                            "and LOWER(RTRIM(LTRIM(Отчество))) = LOWER(RTRIM(LTRIM('" + PersonFio.SecondName + "')))  " +
                            "and ДатаРождения =  '" + PersonFio.DateBirtch + "' " +
                            " update Льготник " +
                            " set[Фамилия] = '" + PersonFio.Famili + "', " +
                            " [Имя] = '" + PersonFio.Name + "', " +
                            " [Отчество] = '" + PersonFio.SecondName + "', " +
                            " [ДатаРождения] = '"+ pf.DateBirtch +"', " + 
                            " [улица] = '"+ pf.улица +"', " +
                            " [НомерДома] = '"+ pf.НомерДома +"', " +
                            "[корпус] = '"+ pf.корпус +"', " +
                            "[НомерКвартиры] = '"+ pf.НомерКвартиры +"', " +
                            "[СерияПаспорта] = '"+ pf.СерияПаспорта +"', " +
                            "[НомерПаспорта] = '"+ pf.НомерПаспорта +"', " +
                            "[ДатаВыдачиПаспорта] = '"+ pf.ДатаВыдачиПаспорта +"', " +
                            "[КемВыданПаспорт] = '"+ pf.КемВыданПаспорт +"', " +
                            "[СерияДокумента] = '"+ pf.СерияДокумента +"', " +
                            "[НомерДокумента] = '"+ pf.НомерДокумента +"', " +
                            "[ДатаВыдачиДокумента] = '"+ pf.ДатаВыдачиДокумента +"', " +
                            "[КемВыданДокумент] = '"+ pf.КемВыданДокумент +"' , " +
                            "id_район  = "+ pf.id_район +" , " +
                            "id_насПункт = "+ pf.id_насПункт +" " +
                            "where[id_льготник] = @id_person__" + count + " " +
                             "INSERT INTO [Договор] ([НомерДоговора] ,[ДатаДоговора] ,[ДатаАктаВыполненныхРабот] " +
                                ",[СуммаАктаВыполненныхРабот] ,[id_льготнаяКатегория] ,[id_поликлинника] ,[Примечание] " +
                                ",[id_комитет] ,[ФлагНаличияДоговора] ,[ФлагНаличияАкта]  ,[id_льготник] ,[ФлагДопСоглашения] " +
                                ",[ДатаЗаписиДоговора] ,[ФлагПроверки] ,logWrite,flagОжиданиеПроверки,idFileRegistProgect,ФлагАнулирован,ФлагВозвратНаДоработку ) " +
                                "VALUES ('" + contract.numContract + "' , '" + contract.DateContract + "' , '" + contract.DataAct + "' , " + contract.SummAct + " " +
                                ",@idЛК_" + count + " ,@IdHosp_" + count + ",'" + contract.Note + "' " +
                                "," + contract.IdConmmite + ",'False' ,'False' " +
                                ",@id_person__" + count + ",'" + contract.numContract + "' " +
                                ",'" + contract.DateWriteContract + "' ,'" + contract.FlagValidate + "' ,'" + contract.logWrite + "','false', " + this.IdFileRegistrProject + ",'false','false' ) " +
                                //" declare @idContract_" + count + " int  " + 
                                " set @idContract_" + count + " = @@IDENTITY" + 
                                " " + QueryInsertServicesContract("@idContract_" + count + " " ) +  // Здесь услуги по договору.
                            "end ";

            return query;
        }


        public string QueryReception(int count)
        {
            string query = " declare @idContract_" + count + " int  " +
                            "declare @IdHosp_" + count + " int " +
                            "select @IdHosp_" + count + " = MAX(id_поликлинника) from Поликлинника " +
                            "where ИНН = '" + hospInn.ИНН + "' " +
                            "if(@IdHosp_" + count + " = 0) " +
                            "begin " +
                                " INSERT INTO [Поликлинника] " +
                                           " ([НаименованиеПоликлинники] " +
                                           " ,[КодПоликлинники] " +
                                           " ,[ЮридическийАдрес] " +
                                           " ,[ФактическийАдрес] " +
                                           " ,[id_главВрач] " +
                                           " ,[id_главБух] " +
                                           " ,[СвидетельствоРегистрации] " +
                                           " ,[ИНН] " +
                                           " ,[КПП] " +
                                           " ,[БИК] " +
                                           " ,[НаименованиеБанка] " +
                                           " ,[РасчётныйСчёт] " +
                                           " ,[ЛицевойСчёт] " +
                                           ",[НомерЛицензии] " +
                                           ",[ДатаРегистрацииЛицензии] " +
                                           ",[ОГРН] " +
                                           ",[СвидетельствоРегистрацииЕГРЮЛ] " +
                                           ",[ОрганВыдавшийЛицензию] " +
                                           ",[Постановление] " +
                                           ",[ОКПО] " +
                                           ",[ОКАТО] " +
                                           ",[Flag] " +
                                           ",[НачальныйНомерДоговора]) " +
                                           "VALUES " +
                                           "('" + dataHospital.НаименованиеПоликлинники.ToString().Trim() + "' " +
                                           ",'" + dataHospital.КодПоликлинники.ToString().Trim() + "' " +
                                           ",'" + dataHospital.ЮридическийАдрес.ToString().Trim() + "' " +
                                           ",'" + dataHospital.ФактическийАдрес.ToString().Trim() + "' " +
                                           ",1 " +
                                           ",1 " +
                                            ",'" + dataHospital.СвидетельствоРегистрации.ToString().Trim() + "' " +
                                            ",'" + dataHospital.ИНН.ToString().Trim() + "' " +
                                           ",'" + dataHospital.КПП.ToString().Trim() + "' " +
                                           ",'" + dataHospital.БИК.ToString().Trim() + "' " +
                                          ",'" + dataHospital.НаименованиеБанка.ToString().Trim() + "' " +
                                           ",'" + dataHospital.РасчётныйСчёт.ToString().Trim() + "' " +
                                          ",'" + dataHospital.ЛицевойСчёт.ToString().Trim() + "' " +
                                           ",'" + dataHospital.НомерЛицензии.ToString().Trim() + "' " +
                                           ",'" + dataHospital.ДатаРегистрацииЛицензии.ToString().Trim() + "' " +
                                           ",'" + dataHospital.ОГРН.ToString().Trim() + "' " +
                                           ",'" + dataHospital.СвидетельствоРегистрацииЕГРЮЛ.ToString().Trim() + "' " +
                                           ",'" + dataHospital.ОрганВыдавшийЛицензию.ToString().Trim() + "' " +
                                           ",'" + dataHospital.Постановление.ToString().Trim() + "' " +
                                           ",'" + dataHospital.ОКПО.ToString().Trim() + "' " +
                                           ",'" + dataHospital.ОКАТО.ToString().Trim() + "' " +
                                           ",'" + dataHospital.Flag + "' " +
                                           "," + dataHospital.НачальныйНомерДоговора + " ) " +
                                           " set @IdHosp_" + count + " = @@IDENTITY " +
                            "end " +
                            "declare @id_насПункт_" + count + " int " +
                            "set @id_насПункт_" + count + " = 0 " +
                            "select @id_насПункт_" + count + " = id_насПункт from НаселённыйПункт " +
                            "where Наименование = '" + NameSity.NameTown + "'  " +
                             " declare @idЛК_" + count + "  int  " +
                        " select @idЛК_" + count + " = id_льготнойКатегории from ЛьготнаяКатегория " +
                        " where LOWER(RTRIM(LTRIM(ЛьготнаяКатегория))) = '" + ЛьготнаяКатегория.ToLower().Trim() + "'  " +
                            "if(select  COUNT(id_договор) from ПриемРеестрвДоговор " +
                            "inner join ПриемРеестровЛьготник " +
                            "ON ПриемРеестровЛьготник.id_льготник = ПриемРеестрвДоговор.id_льготник " +
                            "where LOWER(RTRIM(LTRIM(Фамилия))) = LOWER(RTRIM(LTRIM('" + PersonFio.Famili + "')))  " +
                            "and LOWER(RTRIM(LTRIM(Имя))) = LOWER(RTRIM(LTRIM('" + PersonFio.Name + "')))  " +
                            "and LOWER(RTRIM(LTRIM(Отчество))) = LOWER(RTRIM(LTRIM('" + PersonFio.SecondName + "')))  " +
                            "and ДатаРождения =  '" + PersonFio.DateBirtch + "' " +
                            "and ФлагПроверки = 'True') = 0 " +
                            " begin   " +
                        "if(@id_насПункт_" + count + ") = 0 " +
                        "begin " +
                            "INSERT INTO НаселённыйПункт(Наименование)  " +
                            "VALUES('" + NameSity.NameTown + "')  " +
                        "end " +
                        "declare @idДокумент_" + count + " int  " +
                        "select @idДокумент_" + count + " = id_документ from ТипДокумента " +
                        " where LOWER(RTRIM(LTRIM(НаименованиеТипаДокумента))) = '" + ТипДокумента.ToLower().Trim() + "' " +
                        "end " + // Только что добавил.
                        "if(select  COUNT(id_договор) from ПриемРеестрвДоговор " +
                            "inner join ПриемРеестровЛьготник " +
                            "ON ПриемРеестровЛьготник.id_льготник = ПриемРеестрвДоговор.id_льготник " +
                            "where LOWER(RTRIM(LTRIM(Фамилия))) = LOWER(RTRIM(LTRIM('" + PersonFio.Famili + "')))  " +
                            "and LOWER(RTRIM(LTRIM(Имя))) = LOWER(RTRIM(LTRIM('" + PersonFio.Name + "')))  " +
                            "and LOWER(RTRIM(LTRIM(Отчество))) = LOWER(RTRIM(LTRIM('" + PersonFio.SecondName + "')))  " +
                            "and ДатаРождения =  '" + PersonFio.DateBirtch + "' ) = 0 " +
                            "begin " +
                                " INSERT INTO ПриемРеестровЛьготник ([Фамилия] ,[Имя] ,[Отчество] ,[ДатаРождения] ,[улица] ,[НомерДома] , " +
                                "[корпус] ,[НомерКвартиры] ,[СерияПаспорта] ,[НомерПаспорта] ,[ДатаВыдачиПаспорта] , " +
                                "[КемВыданПаспорт] ,[id_льготнойКатегории] ,[id_документ] ,[СерияДокумента] ,[НомерДокумента] , " +
                                "[ДатаВыдачиДокумента] ,[КемВыданДокумент] ,[id_область] ,[id_район] ,[id_насПункт] ,Снилс) " +
                                "VALUES ('" + pf.Famili + "' " +
                                ",'" + pf.Name + "' " +
                                ",'" + pf.SecondName + "' " +
                                ", '" + pf.DateBirtch + "' " +
                                //", Convert(datetime,'19520324',112)  " + 
                                ",'" + pf.улица + "' " +
                                ",'" + pf.НомерДома + "' " +
                                ",'" + pf.корпус + "' " +
                                ",'" + pf.НомерКвартиры + "'  " +
                                ",'" + pf.СерияПаспорта + "' " +
                                ",'" + pf.НомерПаспорта + "' " +
                                ",'" + pf.ДатаВыдачиПаспорта + "' " +
                                ",'" + pf.КемВыданПаспорт + "'  " +
                                ",@idЛК_" + count + " " +
                                ",@idДокумент_" + count + " " +
                                ",'" + pf.СерияДокумента + "' " +
                                ",'" + pf.НомерДокумента + "' " +
                                ",'" + pf.ДатаВыдачиДокумента + "' ,'" + pf.КемВыданДокумент + "' ,1 , " + pf.id_район + "  " +
                                ",@id_насПункт_" + count + ",'')  " +
                                "INSERT INTO [ПриемРеестрвДоговор] ([НомерДоговора] ,[ДатаДоговора] ,[ДатаАктаВыполненныхРабот] " +
                                ",[СуммаАктаВыполненныхРабот] ,[id_льготнаяКатегория] ,[id_поликлинника] ,[Примечание] " +
                                ",[id_комитет] ,[ФлагНаличияДоговора] ,[ФлагНаличияАкта]  ,[id_льготник] ,[ФлагДопСоглашения] " +
                                ",[ДатаЗаписиДоговора] ,[ФлагПроверки] ,logWrite,flagОжиданиеПроверки, idFileRegistProgect,ФлагАнулирован,ФлагВозвратНаДоработку ) " +
                                "VALUES ('" + contract.numContract + "' , '" + contract.DateContract + "' , '" + contract.DataAct + "' , " + contract.SummAct + " " +
                                ",@idЛК_" + count + " ,@IdHosp_" + count + " ,'" + contract.Note + "' " +
                                "," + contract.IdConmmite + ",'False' ,'False' " +
                                ",@@IDENTITY,'" + contract.numContract + "' " +
                                ",'" + contract.DateWriteContract + "' ,'" + contract.FlagValidate + "' ,'" + contract.logWrite + "','false'," + this.IdFileRegistrProject + ",'false','false' ) " +
                                //" declare @idContract_" + count + " int  " + 
                                " set @idContract_" + count + " = @@IDENTITY" +
                                " " + QueryInsertServicesReceptionContract("@idContract_" + count + " ") +  // Здесь услуги по договору.
                            "end " +
                            //"end " +
                            "else " +
                            "begin " +
                            "declare @id_person__" + count + " int " +
                            "select @id_person__" + count + " = Max(id_льготник) from ПриемРеестровЛьготник " +
                            "where LOWER(RTRIM(LTRIM(Фамилия))) = LOWER(RTRIM(LTRIM('" + PersonFio.Famili + "')))  " +
                            "and LOWER(RTRIM(LTRIM(Имя))) = LOWER(RTRIM(LTRIM('" + PersonFio.Name + "')))  " +
                            "and LOWER(RTRIM(LTRIM(Отчество))) = LOWER(RTRIM(LTRIM('" + PersonFio.SecondName + "')))  " +
                            "and ДатаРождения =  '" + PersonFio.DateBirtch + "' " +
                            " update Льготник " +
                            " set[Фамилия] = '" + PersonFio.Famili + "', " +
                            " [Имя] = '" + PersonFio.Name + "', " +
                            " [Отчество] = '" + PersonFio.SecondName + "', " +
                            " [ДатаРождения] = '" + pf.DateBirtch + "', " +
                            " [улица] = '" + pf.улица + "', " +
                            " [НомерДома] = '" + pf.НомерДома + "', " +
                            "[корпус] = '" + pf.корпус + "', " +
                            "[НомерКвартиры] = '" + pf.НомерКвартиры + "', " +
                            "[СерияПаспорта] = '" + pf.СерияПаспорта + "', " +
                            "[НомерПаспорта] = '" + pf.НомерПаспорта + "', " +
                            "[ДатаВыдачиПаспорта] = '" + pf.ДатаВыдачиПаспорта + "', " +
                            "[КемВыданПаспорт] = '" + pf.КемВыданПаспорт + "', " +
                            "[СерияДокумента] = '" + pf.СерияДокумента + "', " +
                            "[НомерДокумента] = '" + pf.НомерДокумента + "', " +
                            "[ДатаВыдачиДокумента] = '" + pf.ДатаВыдачиДокумента + "', " +
                             "[КемВыданДокумент] = '" + pf.КемВыданДокумент + "' , " +
                            "id_район  = " + pf.id_район + " , " +
                            "id_насПункт = " + pf.id_насПункт + " " +
                            "where[id_льготник] = @id_person__" + count + " " +
                             "INSERT INTO [ПриемРеестрвДоговор] ([НомерДоговора] ,[ДатаДоговора] ,[ДатаАктаВыполненныхРабот] " +
                                ",[СуммаАктаВыполненныхРабот] ,[id_льготнаяКатегория] ,[id_поликлинника] ,[Примечание] " +
                                ",[id_комитет] ,[ФлагНаличияДоговора] ,[ФлагНаличияАкта]  ,[id_льготник] ,[ФлагДопСоглашения] " +
                                ",[ДатаЗаписиДоговора] ,[ФлагПроверки] ,logWrite,flagОжиданиеПроверки,idFileRegistProgect,ФлагАнулирован,ФлагВозвратНаДоработку ) " +
                                "VALUES ('" + contract.numContract + "' , '" + contract.DateContract + "' , '" + contract.DataAct + "' , " + contract.SummAct + " " +
                                ",@idЛК_" + count + " ,@IdHosp_" + count + ",'" + contract.Note + "' " +
                                "," + contract.IdConmmite + ",'False' ,'False' " +
                                ",@id_person__" + count + ",'" + contract.numContract + "' " +
                                ",'" + contract.DateWriteContract + "' ,'" + contract.FlagValidate + "' ,'" + contract.logWrite + "','false', " + this.IdFileRegistrProject + ",'false','false' ) " +
                                //" declare @idContract_" + count + " int  " + 
                                " set @idContract_" + count + " = @@IDENTITY" +
                                " " + QueryInsertServicesReceptionContract("@idContract_" + count + " ") +  // Здесь услуги по договору.
                            "end ";

            return query;
        }
    }
}
