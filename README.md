# TRIdentity API

En: Validate an identity number or verify it using the webservice of the Population and Citizenship Administration of the Republic of Turkey.

Tr: T.C. Kimlik No geçerliliğini kontrol edebilir veya T.C. İçişleri Bakanlığı Nüfus ve Vatandaşlık İşleri Genel Müdürlüğü'nün web servisi ile doğruluğunu kontrol edebilirsiniz.

## Rapid API Hub

<https://rapidapi.com/olcay-fqpXAqccU/api/tridentity>

## Endpoints

### Validate

Check if it is a valid identity number

`GET /idendity/{identityNumber}/validate`

Response:

```json
{
"isValid":true
"message":null
}
```

### Verify

Verify using the webservice

`GET /idendity/{identityNumber}/verify?firstName={firstName}&lastName={lastName}&birthYear={year}`

Response:

```json
{
"isValid":false
"message":"The firstName is required."
}
```

## Tech Stack

- [ASP.NET Core 5.0](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio)
- [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

## Contribution

Open to any contribution.

## License

This software is distributed under [MIT license](LICENSE), so feel free to use it.
