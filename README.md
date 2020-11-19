# revdebug-tutorial-netCore
.NET Core version of web tutorial

## Requirements
    Java version 1.8 or higher
    .NetCore 3.1
    docker
    docker-compose
    
## Preparation
    Open cmd in location desired for sourcecode
    git clone --single-branch --branch feat/multi_service_demo https://github.com/RevDeBug/revdebug-tutorial-netCore
    git clone --single-branch --branch feat/multi_service_demo https://github.com/RevDeBug/revdebug-tutorial-spring
    dotnet nuget add source https://nexus.revdebug.com/repository/nuget -n rdb_nexus
    edit revdebug-tutorial-spring/pom.xml set ArecordServerAddress to desired record server address
    download https://dist.revdebug.com/agent/revdebug-agent-5.7.20.tar.gz and unzip it to revdebug-tutorial-netCore/output/InvoiceJava/app/agent
    
## build
    Open cmd in revdebug-tutorial-netCore
    set REVDEBUG_recordServerAddress=[SERVER ADDRESS]
    build.bat
    
## run
    Open cmd in revdebug-tutorial-netCore/output
    docker-compose up -d
