using Raylib_cs;

GameMode gameMode = GameMode.Default;
const int n = 1000;
const int cellSize = 1;
int cellCount = 0;
int generation = 0;
bool isGameActive = false;
bool isGrid = true;
bool isInfo = true;
Raylib.InitWindow(n + 160, n, "Game of Life");
Raylib.SetTargetFPS(60);
Random rand = new Random();

byte[,] grid = new byte[n, n];
//Cell's values:
//0 - dead
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
    isGameActive = false;
    int x = Raylib.GetMouseX();
    int y = Raylib.GetMouseY();
    x = (int)(Math.Round(x / 5.0) * 5.0);
    y = (int)(Math.Round(y / 5.0) * 5.0);
    if (x < 5 || x > n - 5 || y < 5 || y > n - 5)
        return;
    grid[x, y] = 2;
}
void killCellOnClick()
{
    int x = Raylib.GetMouseX();
    int y = Raylib.GetMouseY();
    x = (int)(Math.Round(x / 5.0) * 5.0);
    y = (int)(Math.Round(y / 5.0) * 5.0);
    if (x < 5 || x > n - 5 || y < 5 || y > n - 5)
        return;
    grid[x, y] = 0;
}
void drawCellsFromGrid()
{
    cellCount = 0;
    for (int i = 5; i <= n - 5; i += 5)
    {
        for (int j = 5; j <= n - 5; j += 5)
        {
            if (grid[i, j] == 2 || grid[i, j] == 3)
                drawCell(i, j);
            else if (grid[i, j] == 4)
                grid[i, j] = 0;
        }
    }
}

void drawCell(int I, int J)
{
    cellCount++;
    
    for (int i = I - cellSize; i <= I + cellSize; i++)
    {
        for (int j = J - cellSize; j <= J + cellSize; j++)
        {
            if (i == I && j == J)
                grid[i, j] = 2;
            Raylib.DrawPixel(i, j, Color.RayWhite);
        }
    }
}

void generateRandomCells()
{
    cellCount = 0;
    initCells();
    isGameActive = false;
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
            if (grid[m, k] == 2 || grid[m, k] == 4)
                neighbours++;
        }
    }

    //original game of life rules
    if(gameMode == GameMode.Default)
    {
        if (neighbours == 3 && grid[I, J] == 0)
            grid[I, J] = 3;
        if ((neighbours < 2 || 3 < neighbours) && grid[I, J] == 2)
            grid[I, J] = 4;
    }
    //life without death's rules
    else if(gameMode == GameMode.LWD)
    {
        if (neighbours == 3 && grid[I, J] == 0)
            grid[I, J] = 3;
    }
    //highlife's rules
    else if(gameMode == GameMode.Highlife)
    {
        if ((neighbours == 3 || neighbours == 6) && grid[I, J] == 0)
            grid[I, J] = 3;
        if ((neighbours < 2 || 3 < neighbours) && grid[I, J] == 2)
            grid[I, J] = 4;
    }
    //day & night's rules
    else if(gameMode == GameMode.DandN)
    {
        if ((neighbours == 3 || neighbours == 6 || neighbours == 7 || neighbours == 8) && grid[I, J] == 0)
            grid[I, J] = 3;
        if (neighbours != 3 && neighbours != 4 && neighbours != 6 && neighbours != 7 && neighbours != 8 && grid[I, J] == 2)
            grid[I, J] = 4;
    }

}
void info()
{
    Raylib.DrawText("Cell count: " + cellCount, n + 5 , 5, 18, Color.Purple);
    Raylib.DrawText("FPS: " + Raylib.GetFPS(), n + 5 , 30, 18, Color.Purple);
    Raylib.DrawText("Generation: " + generation, n + 5 , 55, 18, Color.Purple);
    Raylib.DrawText("R - reset", n + 5 , 80, 18, Color.Purple);
    Raylib.DrawText("G - generate", n + 5 , 105, 18, Color.Purple);
    Raylib.DrawText("Z - grid", n + 5 , 130, 18, Color.Purple);
    Raylib.DrawText("P - info", n + 5 , 155, 18, Color.Purple);
    Raylib.DrawText("Space - on/off", n + 5 , 180, 18, Color.Purple);
    Raylib.DrawText("LMB - add", n + 5, 205, 18, Color.Purple);
    Raylib.DrawText("RMB - kill", n + 5, 230, 18, Color.Purple);
    Raylib.DrawText("Gamemode " + gameMode, n + 5, 250, 18, Color.Purple);
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
        isGameActive = !isGameActive;
    if (Raylib.IsKeyPressed(KeyboardKey.R))
        initCells();
    if (Raylib.IsKeyPressed(KeyboardKey.G))
        generateRandomCells();
    if (Raylib.IsKeyPressed(KeyboardKey.Z))
        isGrid = !isGrid;
    if (Raylib.IsKeyPressed(KeyboardKey.P))
        isInfo = !isInfo;
    if (Raylib.IsKeyPressed(KeyboardKey.One))
        gameMode = GameMode.Default;
    if (Raylib.IsKeyPressed(KeyboardKey.Two))
        gameMode = GameMode.LWD;
    if (Raylib.IsKeyPressed(KeyboardKey.Three))
        gameMode = GameMode.Highlife;
    if (Raylib.IsKeyPressed(KeyboardKey.Four))
        gameMode = GameMode.DandN;
    if (isGameActive)
        play();
    
    drawCellsFromGrid();
    if(isGrid)
        drawGrid();
    if (isInfo)
        info();
    Raylib.EndDrawing();
}

Raylib.CloseWindow();
public enum GameMode
{
    Default = 1,
    LWD = 2,
    Highlife = 3,
    DandN = 4
}

//TODO: launch parameters