cd CustomKnight
msbuild -p:Configuration=Release

cp -R -X ../Releases/Template/ ../Releases/Temp/
mv -f ./bin/Release/CustomKnight.dll ../Releases/Temp/hollow_knight_Data/Managed/Mods/CustomKnight.dll
cd ../Releases/Temp/
zip -r ../CustomKnight.zip .  -x "**/.DS_Store" -x "**/__MACOSX"
cd ../../
openssl sha1 ./Releases/Temp/hollow_knight_Data/Managed/Mods/CustomKnight.dll

