using Raylib_cs;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

const int n = 1000;
const int size = 2;
bool isActive = false;
bool allowGrid = false;
Raylib.InitWindow(n, n, "Game of Life");
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
    for(int i = 0; i < n; i++)
    {
        for(int j = 0; j < n; j++)
        {
            grid[i, j] = 0;
        }
        
    }
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
        for(int j = topHor; j <= botHor; j++)
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
    killCell(x, y);
}
void drawAliveCell()
{
    for (int i = 5; i <= n - 5; i += 5)
    {
        for (int j = 5; j <= n - 5; j += 5)
        {
            if (grid[i, j] == 2 || grid[i, j] == 3)
                addCell(i, j);
            else if (grid[i, j] == 4)
                killCell(i, j);
        }
    }
}

void killCell(int I, int J)
{
    for(int i = I - size; i <= I + size; i++)
    {
        for(int j = J - size; j <= J + size; j++)
        {
            grid[i, j] = 0;
            Raylib.DrawPixel(i, j, Color.RayWhite);
        }
    }
}

void addCell(int I, int J)
{
    for (int i = I - size; i <= I + size; i++)
    {
        for (int j = J - size; j <= J + size; j++)
        {
            if (i == I && j == J)
                grid[i, j] = 2;
            Raylib.DrawPixel(i, j, Color.Black);
        }
    }
}

void generateRandomCells()
{
    initCells();
    isActive = false;
    for (int i = 0; i < 10000; i++)
    {
        int x = (int)(Math.Round(rand.Next(5, n - 5) / 5.0) * 5.0);
        int y = (int)(Math.Round(rand.Next(5, n - 5) / 5.0) * 5.0);
        addCell(x, y);
    }
}
void drawGrid()
{
    for(int i = 4; i < n; i+= 5 )
    {
        Raylib.DrawLine(i, 0, i, n, Color.LightGray);
    }
    for(int j = 3; j < n; j += 5)
    {
        Raylib.DrawLine(0, j, n, j, Color.LightGray);
    }
}
void play()
{
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
            if (i <= 0 || i >= n || j <= 0 || j >= n || (i == I && j == J))
                continue;
            
            if (grid[i, j] == 2 || grid[i, j] == 4)
                neighbours++;
        }
    }
    if (neighbours == 3 && grid[I, J] == 0)
        grid[I, J] = 3;
    if ((neighbours < 2 || 3 < neighbours) && grid[I, J] == 2)
        grid[I, J] = 4;
}

initCells();
while(!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.RayWhite);

    if(Raylib.IsMouseButtonPressed(MouseButton.Left))
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
    if (isActive)
        play();

    drawAliveCell();
    if(allowGrid)
        drawGrid();
    Raylib.EndDrawing();
}

Raylib.CloseWindow();

//TODO: вынос некоторых параметров на экран (fps, кол-во клеток, итд).
//TODO: "бесконечное" поле