using Raylib_cs;

const int n = 1000;
const int size = 2;
int amount = 0;
int generation = 0;
bool isActive = false;
bool allowGrid = true;
bool showInfo = true;
Raylib.InitWindow(n + 160, n, "Game of Life");
Raylib.SetTargetFPS(60);
Random rand = new Random();

byte[,] grid = new byte[n, n];
//0 - dead
//1 - extra pixels for bigger size
//2 - cell's center
//3 - will be populated next generation
//4 - will be dead next generation

void initCells()
{
    generation = 0;
    for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
            grid[i, j] = 0;
}
void addCellOnClick()
{
    isActive = false;
    int x = Raylib.GetMouseX();
    int y = Raylib.GetMouseY();
    x = (int)(Math.Round(x / 5.0) * 5.0);
    y = (int)(Math.Round(y / 5.0) * 5.0);

    //top-left
    int topVert = x - size;
    int topHor = y - size;
    //bottom-right
    int botVert = x + size;
    int botHor = y + size;

    if (topVert < 0 || topHor < 0 || botVert > n || botHor > n ||
        grid[topVert, topHor] == 1 || grid[topVert, topHor] == 2 ||
        grid[botVert, botHor] == 1 || grid[botVert, botHor] == 2)
        return;

    for (int i = topVert; i <= botVert; i++)
    {
        for (int j = topHor; j <= botHor; j++)
        {
            if (i == x && j == y)
                grid[i, j] = 2;
        }
    }
}
void killCellOnClick()
{
    int x = Raylib.GetMouseX();
    int y = Raylib.GetMouseY();
    x = (int)(Math.Round(x / 5.0) * 5.0);
    y = (int)(Math.Round(y / 5.0) * 5.0);
    if (x < 5 || x > n - 5 || y < 5 || y > n - 5)
        return;
    killCell(x, y);
}
void drawCellsFromGrid()
{
    amount = 0;
    for (int i = 5; i <= n - 5; i += 5)
    {
        for (int j = 5; j <= n - 5; j += 5)
        {
            if (grid[i, j] == 2 || grid[i, j] == 3)
                drawCell(i, j);
            else if (grid[i, j] == 4)
                killCell(i, j);
        }
    }
}

void killCell(int I, int J)
{
    grid[I, J] = 0;
}

void drawCell(int I, int J)
{
    amount++;
    
    for (int i = I - size; i <= I + size; i++)
    {
        for (int j = J - size; j <= J + size; j++)
        {
            if (i == I && j == J)
                grid[i, j] = 2;
            Raylib.DrawPixel(i, j, Color.RayWhite);
        }
    }
}

void generateRandomCells()
{
    amount = 0;
    initCells();
    isActive = false;
    //for (int i = 0; i < 10000; i++)
    //{
    //    int x = (int)(Math.Round(rand.Next(5, n - 5) / 5.0) * 5.0);
    //    int y = (int)(Math.Round(rand.Next(5, n - 5) / 5.0) * 5.0);
    //    if (grid[x, y] == 2)
    //        continue;
    //    drawCell(x, y);
    //}
    //"smart" generation
    for (int i = 5; i < n; i += 5)
    {
        for (int j = 5; j < n; j += 5)
        {
            int number = rand.Next(0, 2);
            if (number == 1)
                drawCell(i, j);
        }
    }
}
void drawGrid()
{
    for(int i = 4; i < n; i+= 5 )
    {
        Raylib.DrawLine(i, 3, i, n - 1, Color.DarkGray);
    }
    for(int j = 3; j < n; j += 5)
    {
        Raylib.DrawLine(3, j, n - 1, j, Color.DarkGray);
    }
}
void play()
{
    generation++;
    for(int i = 5; i <= n - 5; i += 5)
    {
        for(int j = 5; j <= n - 5; j += 5)
        {
            fieldTraversal(i, j);
        }
    }
}

void fieldTraversal(int I, int J)
{
    int neighbours = 0;
    for (int i = I - 5; i <= I + 5; i += 5)
    {
        for (int j = J - 5; j <= J + 5; j += 5)
        {
            //non-periodic grid
            //if (i <= 0 || i >= n || j <= 0 || j >= n || (i == I && j == J))
            //    continue;

            if (i == I && j == J)
                continue;
            int m = i;
            int k = j;
            if (i <= 0)
                m = n - 5;
            if (i >= n)
                m = 5;
            if (j <= 0)
                k = n - 5;
            if (j >= n)
                k = 5;
            //for non-periodic grid, change m -> i, k -> j and comment ifs above
            if (grid[m, k] == 2 || grid[m, k] == 4)
                neighbours++;
        }
    }

    //original game of life rules
    if (neighbours == 3 && grid[I, J] == 0)
        grid[I, J] = 3;
    if ((neighbours < 2 || 3 < neighbours) && grid[I, J] == 2)
        grid[I, J] = 4;

    //life without death's rules
    //if (neighbours == 3 && grid[I, J] == 0)
    //    grid[I, J] = 3;

    //highlife's rules
    //if ((neighbours == 3 || neighbours == 6) && grid[I, J] == 0)
    //    grid[I, J] = 3;
    //if ((neighbours < 2 || 3 < neighbours) && grid[I, J] == 2)
    //    grid[I, J] = 4;

    //day & night's rules
    //if ((neighbours == 3 || neighbours == 6 || neighbours == 7 || neighbours == 8) && grid[I, J] == 0)
    //    grid[I, J] = 3;
    //if (neighbours != 3 && neighbours != 4 && neighbours != 6 && neighbours != 7 && neighbours != 8 && grid[I, J] == 2)
    //    grid[I, J] = 4;
}
void info()
{
    Raylib.DrawText("Cell amount: " + amount, n + 5 , 5, 18, Color.Purple);
    Raylib.DrawText("FPS: " + Raylib.GetFPS(), n + 5 , 30, 18, Color.Purple);
    Raylib.DrawText("Generation: " + generation, n + 5 , 55, 18, Color.Purple);
    Raylib.DrawText("R - reset", n + 5 , 80, 18, Color.Purple);
    Raylib.DrawText("G - generate", n + 5 , 105, 18, Color.Purple);
    Raylib.DrawText("Z - grid", n + 5 , 130, 18, Color.Purple);
    Raylib.DrawText("P - info", n + 5 , 155, 18, Color.Purple);
    Raylib.DrawText("Space - on/off", n + 5 , 180, 18, Color.Purple);
    Raylib.DrawText("LMB - add", n + 5, 205, 18, Color.Purple);
    Raylib.DrawText("RMB - kill", n + 5, 230, 18, Color.Purple);


}

initCells();
while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);

    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        addCellOnClick();

    if(Raylib.IsMouseButtonPressed(MouseButton.Right))
        killCellOnClick();
    
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
        isActive = !isActive;
    if (Raylib.IsKeyPressed(KeyboardKey.R))
        initCells();
    if (Raylib.IsKeyPressed(KeyboardKey.G))
        generateRandomCells();
    if (Raylib.IsKeyPressed(KeyboardKey.Z))
        allowGrid = !allowGrid;
    if (Raylib.IsKeyPressed(KeyboardKey.P))
        showInfo = !showInfo;
    if (isActive)
        play();
    
    drawCellsFromGrid();
    if(allowGrid)
        drawGrid();
    if (showInfo)
        info();
    Raylib.EndDrawing();
}

Raylib.CloseWindow();


//TODO: передавать некоторые значения через параметры запуска