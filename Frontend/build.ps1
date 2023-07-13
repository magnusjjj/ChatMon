Remove-Item '..\publish' -Recurse
npm run build
Compress-Archive '..\publish' '..\chatmon.zip'