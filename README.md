# Inmobiliaria

## Integrantes:
- Ramiro Romero
- Nicolas Bustamante
## Admin
- email: admin@inmobiliaria.com
- contraseña: Admin123!
## Diagramas

(Estado actual del proyecto, seran ampliados a medida que el scope crezca)

### Entidad Relacion
<img width="1031" height="771" alt="Inmobiliaria Lab 2 - Romero Bustamante drawio" src="https://github.com/user-attachments/assets/2ee4bee5-3ee8-498b-85e8-c5876df6a3a0" />

## Funcionalidades

- ABM completo de Propietarios, Inquilinos, Inmuebles, Tipos de Inmueble y Reservas
- Gestión de Pagos por reserva, con edición restringida (solo concepto) y anulación como cambio de estado
- Autenticación con usuario/contraseña y roles Administrador/Empleado, con auditoría de creación y anulación en reservas y pagos
- Baja lógica de propietarios, inquilinos e inmuebles
- Suspensión temporal de la oferta de un inmueble
- Validación de solapamiento de fechas al crear/editar reservas
- Búsqueda de inmuebles disponibles por rango de fechas y tipo
- Finalización anticipada de reservas con cálculo automático de multa (25%/50%)
- Renovación/extensión de reservas sin modificar la original
- Imágenes de inmuebles: portada y galería, con validación de formato, tamaño y dimensiones
- Informes: inmuebles por propietario y disponibilidad, inmuebles más reservados, inmuebles sin reservas, reservas vigentes, reservas próximas a finalizar, inmuebles libres entre fechas
- Paginado por servidor en los listados principales

### Sin terminar (Poco tiempo + mal manejo del mismo. 100% mi culpa)

- Selects con búsqueda/filtro resuelta en el servidor (autocompletado) en los combos de propietario, inmueble e inquilino — hoy cargan todos los registros disponibles
- Búsqueda por texto resuelta en el servidor en los listados principales (Propietarios, Inquilinos, Inmuebles, Reservas) — actualmente solo cuentan con paginado, sin filtro de búsqueda
    - Realmente lo deje para el ultimo porque no estaba seguro de que se pedia con la busqueda desde el servidor, pero es posible que sea esto de busqueda por texto y de ser asi no esta implementado

## Instrucciones SQL

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
