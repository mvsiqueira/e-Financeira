# EFinanceiraApp

Aplicativo Windows para:

- assinar um XML de lote da e-Financeira com um certificado instalado no Windows;
- criptografar o XML assinado com um certificado informado em arquivo `.cer`.

## Projeto

- Solucao principal: `C:\Users\mvsiq\Downloads\apps\efinanceira\EFinanceiraApp\EFinanceiraApp.csproj`
- Executavel compilado em modo Debug: `C:\Users\mvsiq\Downloads\apps\efinanceira\EFinanceiraApp\bin\Debug\net10.0-windows\EFinanceiraApp.exe`

## Como usar

1. Clique em `Selecionar XML` e escolha o lote XML.
2. Clique em `Escolher instalado` e selecione o certificado com chave privada para assinatura.
3. Clique em `1. Assinar XML`.
4. Clique em `Selecionar .cer` e escolha o certificado publico para criptografia.
5. Clique em `2. Criptografar XML assinado`.

## Arquivos gerados

- XML assinado: mesmo nome do arquivo original com sufixo `-ASSINADO.xml`
- XML criptografado: mesmo nome do arquivo assinado com sufixo `-Criptografado.xml`

## Observacoes

- A assinatura segue a logica do exemplo `ExemploAssinadorXML_256bytes`.
- A criptografia segue a logica do exemplo `ExemploCriptografiaLoteEFinanceira`.
- O app aceita lotes com eventos em `loteEventosAssincrono/eventos/evento` e tambem tenta `loteEventos/evento`.
