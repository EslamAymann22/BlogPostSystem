1- build your solution layers 
    <br>-Apis
    <br>-Core
    <br>-Repository
    <br>-Service

2- make classes (DataBase)

3- Create DbContext => make ur database , install (entity.framworks.core) then make constractor for DbContext , make options for DbContextOptions int program file 
to ask CLR to ingict an opject from  DbContextOptions<-MyDataBaseName->
, make my classes that will be table in my database
, make configurations in my classes (My relations and key ...) so we make folder "Config" to add all configurations
, after that we make migrations .
, make the update auto by code in main with get scops from app and get services and getRequiredServes then make .databse.migrate 
, use try catch with logger factory like the database update mean use services.geatrequired>ILoggerFactoruy< 
