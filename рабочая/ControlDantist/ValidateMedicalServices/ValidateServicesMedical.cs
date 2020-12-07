using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.ClassValidRegions;

namespace ControlDantist.ValidateMedicalServices
{
    public class ValidateServicesMedical
    {
        List<PersonValidEsrn> listPersonValidEsrn;
        public ValidateServicesMedical(List<PersonValidEsrn> listPersonValidEsrn)
        {
            if(listPersonValidEsrn.Count() > 0)
            this.listPersonValidEsrn = listPersonValidEsrn;
        }

        public string ValidateServer(string номерДоговора)
        {

            //string query = @"select TabS.ВидУслуги,TabS.Цена,TF.НаименованиеУслуги,TF.цена from (
            //                select ВидУслуги, Цена from ВидУслуг
            //                 inner join Договор
            //                 ON ВидУслуг.id_поликлинника = Договор.id_поликлинника
            //                 where НомерДоговора = '" + номерДоговора + "') as TabS " +
            //                @" inner join(select НаименованиеУслуги, цена, COUNT(НаименованиеУслуги) as Количество,SUM(Сумма) as Сумма from УслугиПоДоговору
            //                                             inner join Договор
            //                                             ON Договор.id_договор = УслугиПоДоговору.id_договор
            //                                             where НомерДоговора = '" + номерДоговора + "' and Договор.idFileRegistProgect is not null  and flagОжиданиеПроверки = 0 " +
            //                                            @" group by НаименованиеУслуги,цена) as TF
            //                on LOWER(RTRIM(LTRIM(TabS.ВидУслуги))) = LOWER(RTRIM(LTRIM(TF.НаименованиеУслуги))) and TabS.Цена = TF.цена 
            //                group by  TabS.ВидУслуги,TabS.Цена,TF.НаименованиеУслуги,TF.цена ";

            string query = @"select TabS.ВидУслуги,TabS.Цена,TF.НаименованиеУслуги,TF.цена from (
                            select ВидУслуги, Цена from ВидУслуг
                            inner join (
                            select MAX(id_поликлинника) as id_поликлинника from Поликлинника
                            inner join (
                            select ИНН from Поликлинника
                            inner join Договор
                            ON Поликлинника.id_поликлинника = Договор.id_поликлинника
                            where Договор.НомерДоговора = '" + номерДоговора + "') as TempHosp " +
                            @" ON TempHosp.ИНН = Поликлинника.ИНН
                            group by Поликлинника.ИНН) as TabServer
                            ON ВидУслуг.id_поликлинника = TabServer.id_поликлинника ) as TabS
                            inner join(select НаименованиеУслуги, цена, COUNT(НаименованиеУслуги) as Количество,SUM(Сумма) as Сумма from УслугиПоДоговору
                            inner join Договор
                            ON Договор.id_договор = УслугиПоДоговору.id_договор
                            where НомерДоговора = '" + номерДоговора + "' and Договор.idFileRegistProgect is not null and flagОжиданиеПроверки = 0 " +
                            @" group by НаименованиеУслуги,цена
                            ) as TF
                            on LOWER(RTRIM(LTRIM(TabS.ВидУслуги))) = LOWER(RTRIM(LTRIM(TF.НаименованиеУслуги))) and TabS.Цена = TF.цена
                                                        group by  TabS.ВидУслуги,TabS.Цена,TF.НаименованиеУслуги,TF.цена ";

            return query;
        }

        /// <summary>
        /// Услуги в договоре.
        /// </summary>
        /// <param name="номерДоговора"></param>
        /// <returns></returns>
        public string ValidateContract(string номерДоговора)
        {
            string query = @"select НаименованиеУслуги,цена from (
                            select НаименованиеУслуги,цена from УслугиПоДоговору
                            inner join Договор
                            ON Договор.id_договор = УслугиПоДоговору.id_договор
                            where LOWER(RTRIM(LTRIM(НомерДоговора))) = LOWER(RTRIM(LTRIM('" + номерДоговора + "'))) and Договор.idFileRegistProgect is not null  and flagОжиданиеПроверки = 0 " +
                            " ) as Tfile " +
                            " group by НаименованиеУслуги,цена ";

            return query;
        }

        public string ValidateJoin(string номерДоговора)
        {
            string innerJoin = " select TabServer.ВидУслуги,TabServer.Цена from ( " +
                           " select TabServer.ВидУслуги, TabServer.Цена from ( " +
                          " select [ВидУслуги],Tab3.Цена from ( " +
                          " select[ВидУслуги],Цена from ВидУслуг " +
                          " inner join( " +
                          " select Tab1.ИНН, MAX(Поликлинника.id_поликлинника) as idHospital  from Поликлинника " +
                          " inner join (select Поликлинника.ИНН, НомерДоговора from Договор " +
                          " inner join Поликлинника " +
                          " ON Поликлинника.id_поликлинника = Договор.id_поликлинника " +
                          " where НомерДоговора = '" + номерДоговора + "' ) as Tab1 " +
                          " ON Tab1.ИНН = Поликлинника.ИНН " +
                          " GROUP BY Tab1.ИНН) as TabTemp " +
                          " ON TabTemp.idHospital = ВидУслуг.id_поликлинника) as Tab3 " +
                          " group by [ВидУслуги],Tab3.Цена) as TabServer " +
                          " inner join( " +
                          " select[НаименованиеУслуги], [цена] from Договор " +
                          " inner join УслугиПоДоговору " +
                          " ON Договор.id_договор = УслугиПоДоговору.id_договор " +
                          " where НомерДоговора = '" + номерДоговора + "' and ФлагВозвратНаДоработку = 0 and ФлагАнулирован = 0 and flagОжиданиеПроверки = 0 " +
                          " and idFileRegistProgect is not null) as Tab2 " +
                          " ON LTRIM(RTRIM(LOWER(REPLACE(TabServer.ВидУслуги, ' ', '')))) = LTRIM(RTRIM(LOWER(REPLACE(Tab2.НаименованиеУслуги, ' ', '')))) " +
                          " and TabServer.Цена = Tab2.[цена] ) as TabServer " +
                          "   inner join ( " +
                          " select НаименованиеУслуги,цена from УслугиПоДоговору " +
                          " inner join Договор " +
                          " ON Договор.id_договор = УслугиПоДоговору.id_договор " +
                          " where НомерДоговора = '" + номерДоговора + "' and Договор.idFileRegistProgect is not null  and flagОжиданиеПроверки = 0 " +
                          " ) asTabContract " +
                            " ON TabServer.ВидУслуги = asTabContract.НаименованиеУслуги and TabServer.Цена = asTabContract.цена ";

            return innerJoin;
        }
    }
}
