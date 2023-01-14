FROM microsoft/dotnet-framework:4.7
ARG source
WORKDIR /app
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["C:\\app\\WinMan.exe"]
