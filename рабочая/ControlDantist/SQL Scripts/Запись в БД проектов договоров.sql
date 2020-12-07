use Dentists
begin transaction

-- переменная для хранения количества договоров.
declare @countContract int
--declare @countThaun int
-- Проверим есть ли такой договор прошедший проверку
select @countContract = COUNT(*)  from Договор 
where НомерДоговора = '2-2/5556' and id_льготник in 
(select id_льготник from Льготник where Фамилия = 'Соломина' and Имя = 'Нина' and Отчество = 'Ивановна' and ДатаРождения =  '19360310' and ФлагПроверки = 'True') 

-- Проверим есть ли договра которые не прошли проверку
if(@countContract = 0)
begin
	declare @countThaun int
	select @countThaun = count(Наименование) from НаселённыйПункт where Наименование = 'Саратов'
	if(@countThaun = 0)
	begin 
		INSERT INTO НаселённыйПункт(Наименование)VALUES('Саратов') 
	end
	else
	declare @countRaion int
	 --Проверим есть ли в базе данных район
	select @countRaion = count(РайонОбласти) from НаименованиеРайона where РайонОбласти = ''
	if(@countRaion = 0)
	begin
		
	end
	
end
else
begin 
print('Не пишем в базу данных')
end



rollback transaction