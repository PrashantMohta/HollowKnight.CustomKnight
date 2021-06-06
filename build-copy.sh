cd CustomKnight
msbuild -p:Configuration=Debug
mv -f ./bin/Debug/CustomKnight.dll ../../linkedtoHKMods/CustomKnight.dll
cd ..