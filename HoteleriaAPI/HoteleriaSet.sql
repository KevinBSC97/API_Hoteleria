CREATE OR ALTER PROCEDURE sp_HoteleriaSet
	@iTransaction				AS VARCHAR(100),
	@iXML						AS XML
AS

BEGIN
	
	SET NOCOUNT ON;

	DECLARE @respuesta			AS VARCHAR(10);
	DECLARE @leyenda			AS VARCHAR(20);

	-------------------------------------------

	DECLARE @Nombre				AS VARCHAR(50);
	DECLARE @Correo				AS VARCHAR(50);
	DECLARE @NumeroTelefono		AS VARCHAR(50);
	DECLARE @Mensaje			AS VARCHAR(50);

	-------------------------------------------

	BEGIN TRY
			BEGIN TRANSACTION TRX_DATOS

						IF(@iTransaction = 'sp_Contacto')
							BEGIN
								SELECT
										@Nombre			=LTRIM(DATO_XML.X.value('Nombre[1]', 'VARCHAR(50)')),
										@Correo			=LTRIM(DATO_XML.X.value('Correo[1]','VARCHAR(50)')),
										@NumeroTelefono =LTRIM(DATO_XML.X.value('NumeroTelefono[1]','VARCHAR(50)')),
										@Mensaje		=LTRIM(DATO_XML.X.value('Mensaje[1]','VARCHAR(50)'))

								FROM @iXML.nodes('/Contacto') AS DATO_XML(X);
								insert into Contacto (nombre, correo, numero_telefono, mensaje) VALUES (@Nombre,
																			@Correo, @NumeroTelefono, @Mensaje);

								COMMIT TRANSACTION TRX_DATOS;
								SET @respuesta = 'OK';
								SET @leyenda = 'Datos ingresados correctamente';

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