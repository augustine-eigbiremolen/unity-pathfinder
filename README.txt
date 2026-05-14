1. Unity 2022 was used.
2. The automated delivery bus calculates the minimum cost from chosen start to end, and choose the fastest, hence factors like traffic, obstacles, bad road networks and distance are taken into consideration.
3. To disable the time factor, select the delivery bus robot and turn of "Simulate Time Cost". Only the distance will be used to find the optimal path.
4. It also find a better route while on the move. To simulate dynamic routing at runtime, check the "Activate Runtime Reroute" box. 

nb: Do not combine -quit with -runTests here; it can shut down before the test runner’s first Update. Use -batchmode -nographics and let -runTests exit the process.
Only one Unity instance may have the project open; a second batch run will fail with “another Unity instance”.