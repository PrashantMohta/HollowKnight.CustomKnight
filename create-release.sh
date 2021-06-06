cd CustomKnight
msbuild -p:Configuration=Release

cp -R -X ../Releases/Template/ ../Releases/Temp/
mv -f ./bin/Release/CustomKnight.dll ../Releases/Temp/hollow_knight_Data/Managed/Mods/CustomKnight/CustomKnight.dll
cd ../Releases
zip -r ./CustomKnight.zip ./Temp/.  -x "**/.DS_Store" -x "**/__MACOSX"


