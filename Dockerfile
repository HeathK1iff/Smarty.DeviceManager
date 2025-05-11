FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app
EXPOSE 5005
COPY . .
RUN dotnet restore "Smarty.DeviceManager.Application/Smarty.DeviceManager.Application.csproj"
RUN dotnet build "Smarty.DeviceManager.Application/Smarty.DeviceManager.Application.csproj" -c Release -o /app/build
RUN dotnet publish "Smarty.DeviceManager.Application/Smarty.DeviceManager.Application.csproj" -c Release -o /app/publish /p:UseAppHost=false
ENTRYPOINT ["dotnet", "/app/publish/Smarty.DeviceManager.Application.dll"]