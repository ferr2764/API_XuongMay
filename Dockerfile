# Sử dụng hình ảnh .NET SDK 8.0 để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy file .csproj và khôi phục dependencies trước
COPY bookify-api/*.csproj ./bookify-api/
RUN dotnet restore bookify-api/bookify-api.csproj

# Copy toàn bộ source code vào container
COPY . .

# Build ứng dụng ở chế độ Release
RUN dotnet publish bookify-api/bookify-api.csproj -c Release -o /out

# Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy từ bước build sang runtime container
COPY --from=build /out ./

# Mở cổng 80 và 443 cho container
EXPOSE 80
EXPOSE 443

# Chạy ứng dụng khi container khởi động
ENTRYPOINT ["dotnet", "bookify-api.dll"]
