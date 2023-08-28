CREATE OR ALTER PROCEDURE sp_HoteleriaGet
	@iTransaction				AS VARCHAR(100),
	@iXML						AS XML
AS

BEGIN
	
	SET NOCOUNT ON;

	DECLARE @respuesta			AS VARCHAR(10);
	DECLARE @leyenda			AS VARCHAR(20);
	
	-------------------------------------------

	DECLARE @Id					AS INT;
	DECLARE @NombreUsuario		AS VARCHAR(50);
	DECLARE @Contraseña			AS VARCHAR(50);

	DECLARE @NUsuario				AS INT;

	-------------------------------------------

	DECLARE @Calificacion		AS FLOAT;
	DECLARE @Descripcion		AS VARCHAR(50);
	DECLARE @Precio				AS DECIMAL(10, 2);
	DECLARE @Imagen				AS VARCHAR(100);

	DECLARE @NServicios			AS INT;

	-------------------------------------------


	BEGIN TRY
			BEGIN TRANSACTION TRX_DATOS

					IF(@iTransaction = 'sp_Login')
						BEGIN
							
							SELECT @NombreUsuario = DATO_XML.X.value('NombreUsuario[1]','VARCHAR(50)'),
									@Contraseña = DATO_XML.X.value('Contraseña[1]','VARCHAR(50)')
							FROM @iXML.nodes('/Usuario') AS DATO_XML(X)

							SELECT *
							FROM Usuarios u WHERE u.nombre_usuario = @NombreUsuario and u.contraseña = @Contraseña
							
							SET @respuesta = 'Datos correctos';
							SET @leyenda = 'Credenciales correctas';

						END

					IF(@iTransaction = 'sp_ServicioHotel')
						BEGIN

							SELECT
									id,
									calificacion,
									descripcion,
									precio,
									imagen
							FROM Servicios_Hotel;

							SET @respuesta = 'Datos Obtenidos';
							SET @leyenda = 'Datos de la tabla Servicios_Hotel obtenidos';

							COMMIT TRANSACTION TRX_DATOS;

						END

						IF(@iTransaction = 'sp_HabitacionHotel')
						BEGIN

							SELECT
									id,
									calificacion,
									nombre_habitacion,
									descripcion,
									precio,
									imagen
							FROM Habitaciones;

							SET @respuesta = 'Datos Obtenidos';
							SET @leyenda = 'Datos de la tabla Habitaciones obtenidos';

							COMMIT TRANSACTION TRX_DATOS;

						END
	END TRY

	BEGIN CATCH
		IF @@TRANCOUNT > 0
			BEGIN
				ROLLBACK TRANSACTION TRX_DATOS;
			END

		SET @respuesta = 'Error';
		SET @leyenda = 'Error al realizar la transacción '+@iTransaction+' - Error: '+ ERROR_MESSAGE();
	END CATCH

	SELECT @respuesta AS Respuesta, @leyenda AS Legenda;

END