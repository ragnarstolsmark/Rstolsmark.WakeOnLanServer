FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG VERSION
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build app
COPY / ./
RUN dotnet publish -c Release -p:Version=$VERSION -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
VOLUME /data
ENTRYPOINT ["dotnet", "Rstolsmark.WakeOnLanServer.dll"]
# Run with:
# docker run -d \
# -p 80:80 \
# -e ASPNETCORE_PasswordAuthenticationOptions__HashedPassword=AQAAAAEAACcQAAAAEKf2sSufN9V6t+tENnq9sAE/IOhmb1PQzTAOdLXcUj3PPJuVkx6Ku/2De4jb1NCdeA== \
# -e ASPNETCORE_PasswordAuthenticationOptions__Realm="Wake me up" \
# --name wakeonlanserver \
# --mount source=wakeonlanserver,target=/data \
# rstolsmark/wakeonlanserver
#
# Backup (backup is stored as backup.tar in current directory)
# docker run --rm --volumes-from wakeonlanserver -v $(pwd):/backup ubuntu tar cvf /backup/backup.tar /data
#
# Restore (assume backup.tar is stored in current directory)
# docker run --rm --volumes-from wakeonlanserver -v $(pwd):/backup ubuntu bash -c "cd /data && tar xvf /backup/backup.tar --strip 1"