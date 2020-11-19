xcopy docker output\ /E /Y
cd ../revdebug-tutorial-spring
call ./mvnw package -DskipTests
xcopy target\revdebug-tutorial-spring-0.0.1-SNAPSHOT.jar ..\revdebug-tutorial-netCore\output\InvoiceJava\app\ /Y
cd ../revdebug-tutorial-netCore
cd Discounter
dotnet clean
dotnet publish -o ../output/Discounter/app
cd ../InvoiceAssembler
dotnet clean
dotnet publish -o ../output/InvoiceAssembler/app
cd ../InvoicesCore
dotnet clean
dotnet publish -o ../output/InvoiceCore/app
cd ../InvoiceSender
dotnet clean
dotnet publish -o ../output/InvoiceSender/app
