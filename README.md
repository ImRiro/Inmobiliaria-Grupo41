# Inmobiliaria

## Integrantes:
- Ramiro Romero
- Nicolas Bustamante
## Diagramas

(Estado actual del proyecto, seran ampliados a medida que el scope crezca)

### Casos de Uso
https://www.figma.com/files/team/1614361052615337445/project/639110759?fuid=1614361051090803016

### Entidad Relacion
<img width="1031" height="771" alt="Inmobiliaria Lab 2 - Romero Bustamante drawio" src="https://github.com/user-attachments/assets/2ee4bee5-3ee8-498b-85e8-c5876df6a3a0" />

### Instrucciones SQL

1. Abrir MySQL y conectarse al servidor local (localhost:3306).
2. Ejecutar el script "Inmobiliaria-lab2-RomeroBustamante.sql" incluido en este repositorio
    - Si estas usando DBeaver como es mi caso tenes que crear una nueva Base de Datos, darle segundo click, ir a tools -> Restore Database, ahi te abre una ventana y en el campo "input" pones el archivo.
    - En el caso de XAMPP tambien deberas crear una base de datos previamente y luego importar el archivo.
3. Configurar la cadena de conexión utilizando User Secrets. Esto permite almacenar información sensible, como el usuario y contraseña de MySQL, sin incluirla directamente en el repositorio.

4. Desde la carpeta raíz del proyecto, configurar la cadena de conexión con el siguiente comando, reemplazando los valores por los correspondientes a tu instalación de MySQL:

    `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=TU_BD;User=TU_USUARIO;Password=TU_CONTRASEÑA;"`
    
    Por ejemplo:

    `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=Inmobiliaria;User=root;Password=1234;"`
5. Y eso debería ser suficiente. Una vez configurada correctamente la cadena de conexión, la aplicación debería poder conectarse a la base de datos importada.
