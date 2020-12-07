
declare @DateStart Date
set @DateStart = '20150101'
declare @DateEnd Date
set @DateEnd = '20151231'
SELECT     derivedtbl_1.F2 AS НаименованиеПоликлинники, derivedtbl_1.Expr1 AS Сумма, derivedtbl_2.Expr1 AS ВетеранТрудаСумма, 
                      derivedtbl_3.Expr1 AS ВетеранВоеннойСлужбыСумма, derivedtbl_4.Expr1 AS ВетеранТрудаСаратовскойОбластиСумма, 
                      derivedtbl_5.Expr1 AS ТруженникТылаСумма, derivedtbl_6.Expr1 AS РеабелитированныеСумма
FROM         (SELECT     F2, SUM(Сумма) AS Expr1
                       FROM          dbo.ViewСуммаНаДоговора
                       WHERE      (ДатаЗаписиДоговора >= @DateStart) AND (ДатаЗаписиДоговора <= @DateEnd)
                       GROUP BY F2) AS derivedtbl_1 LEFT OUTER JOIN
                          (SELECT     ЛьготнаяКатегория, SUM(Сумма) AS Expr1, F2
                            FROM          dbo.ViewСуммаНаДоговора AS ViewСуммаНаДоговора_5
                            WHERE      (ЛьготнаяКатегория = 'Реабилитированные лица') AND (ДатаЗаписиДоговора >= @DateStart) AND 
                                                   (ДатаЗаписиДоговора <= @DateEnd)
                            GROUP BY ЛьготнаяКатегория, F2) AS derivedtbl_6 ON derivedtbl_1.F2 = derivedtbl_6.F2 LEFT OUTER JOIN
                          (SELECT     ЛьготнаяКатегория, SUM(Сумма) AS Expr1, F2
                            FROM          dbo.ViewСуммаНаДоговора AS ViewСуммаНаДоговора_4
                            WHERE      (ЛьготнаяКатегория = 'Труженик тыла') AND (ДатаЗаписиДоговора >= @DateStart) AND (ДатаЗаписиДоговора <= @DateEnd)
                            GROUP BY ЛьготнаяКатегория, F2) AS derivedtbl_5 ON derivedtbl_1.F2 = derivedtbl_5.F2 LEFT OUTER JOIN
                          (SELECT     ЛьготнаяКатегория, SUM(Сумма) AS Expr1, F2
                            FROM          dbo.ViewСуммаНаДоговора AS ViewСуммаНаДоговора_3
                            WHERE      (ЛьготнаяКатегория = 'Ветеран труда Саратовской области') AND (ДатаЗаписиДоговора >= @DateStart) AND 
                                                   (ДатаЗаписиДоговора <= @DateEnd)
                            GROUP BY ЛьготнаяКатегория, F2) AS derivedtbl_4 ON derivedtbl_1.F2 = derivedtbl_4.F2 LEFT OUTER JOIN
                          (SELECT     ЛьготнаяКатегория, SUM(Сумма) AS Expr1, F2
                            FROM          dbo.ViewСуммаНаДоговора AS ViewСуммаНаДоговора_2
                            WHERE      (ЛьготнаяКатегория = 'Ветеран  военной службы') AND (ДатаЗаписиДоговора >= @DateStart) AND 
                                                   (ДатаЗаписиДоговора <= @DateEnd)
                            GROUP BY ЛьготнаяКатегория, F2) AS derivedtbl_3 ON derivedtbl_1.F2 = derivedtbl_3.F2 LEFT OUTER JOIN
                          (SELECT     ЛьготнаяКатегория, SUM(Сумма) AS Expr1, F2
                            FROM          dbo.ViewСуммаНаДоговора AS ViewСуммаНаДоговора_1
                            WHERE      (ЛьготнаяКатегория = 'Ветеран труда') AND (ДатаЗаписиДоговора >= @DateStart) AND (ДатаЗаписиДоговора <= @DateEnd)
                            GROUP BY ЛьготнаяКатегория, F2) AS derivedtbl_2 ON derivedtbl_1.F2 = derivedtbl_2.F2