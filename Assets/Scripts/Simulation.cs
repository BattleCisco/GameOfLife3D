using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation
{
    GameManager gameManager;

    public int generation = 0;
    public int deadCells = 0;
    public Cell[] cells;

    public Simulation(GameManager gameManager) {
        this.gameManager = gameManager;

        this.cells = new Cell[this.gameManager.cellsInColumn * this.gameManager.cellsInRow];

        for (int y = 0; y < this.gameManager.cellsInRow; y++)
            for (int x = 0; x < this.gameManager.cellsInColumn; x++)
            {
                this.setCell(new Cell(x, y, new Color(0, 0, 0), false));
                if (Random.Range(0, 1) < this.gameManager.fillPercentage)
                {
                    Cell cell = this.getCell(x, y);
                    cell.alive = true;
                    cell.cellColor = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1f);
                }
            }
    }

    public Simulation(Simulation simulation) {
        this.gameManager = simulation.gameManager;
        this.generation = simulation.generation;
        this.deadCells = simulation.deadCells;
        this.cells = simulation.cells;
    }

    public Cell getCell(int x, int y) { return this.cells[x + this.gameManager.cellsInColumn * y]; }
    public void setCell(Cell cell) {
        //Debug.Log("Setting cell (" + cell.x + ", " + cell.y + ")");
        this.cells[cell.x + this.gameManager.cellsInRow * cell.y] = cell;
    }

    public void setCell(int x, int y, bool alive) {
        Color randomColor = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1f);
        Cell cell = new Cell(x, y, randomColor, alive);
        this.setCell(cell);
    }

    public void setCell(int x, int y, Color c, bool alive) {
        Cell cell = new Cell(x, y, c, alive);
        this.setCell(cell);
    }

    public int getAliveNeighbors(Cell cell) {
        int numberOfAliveNeighbors = 0;
        for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
                if (dy != 0 || dx != 0)
                    if (cell.x + dx >= 0 && cell.x + dx < this.gameManager.cellsInColumn)
                        if (cell.y + dy >= 0 && cell.y + dy < this.gameManager.cellsInRow)
                            if (this.getCell(cell.x + dx, cell.y + dy).alive)
                                numberOfAliveNeighbors += 1;
        return numberOfAliveNeighbors;
    }

    public List<Cell> getChangedCells()
    {
        List<Cell> changedCells = new List<Cell>();

        for (int y = 0; y < this.gameManager.cellsInRow; ++y)
            for (int x = 0; x < this.gameManager.cellsInColumn; ++x)
            {
                int numberOfAliveNeighbors = getAliveNeighbors(this.getCell(x, y));

                Cell possiblyChangedCell;
                if (this.getCell(x, y).alive)
                {
                    if (numberOfAliveNeighbors == 2 || numberOfAliveNeighbors == 3)
                        possiblyChangedCell = this.getCell(x, y);
                    else
                        possiblyChangedCell = new Cell(x, y, this.getCell(x, y).cellColor, false);
                }
                else
                {
                    if (numberOfAliveNeighbors == 3)
                    {
                        Color c;
                        if (Random.Range(0, 1) < this.gameManager.chanceOfMutation)
                        {
                            c = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1f);
                        }
                        else
                        {
                            c = getInheritedColor(x, y);
                        }
                        possiblyChangedCell = new Cell(x, y, c, true);
                    }
                    else
                        possiblyChangedCell = this.getCell(x, y);
                }

                if (this.getCell(x, y).alive ^ possiblyChangedCell.alive)
                    changedCells.Add(possiblyChangedCell);
            }
        
        return changedCells;
    }

    public List<Cell> getAliveCells()
    {
        List<Cell> aliveCells = new List<Cell>();
        
        for (int y = 0; y < this.gameManager.cellsInRow; ++y)
            for (int x = 0; x < this.gameManager.cellsInColumn; ++x)
            {
                Cell cell = this.getCell(x, y);

                if (cell.alive)
                    aliveCells.Add(cell);
            }

        return aliveCells;
    }

    public Color getInheritedColor(int x, int y)
    {
        List<Cell> aliveCells = this.getAliveCells();

        float redSum = 0, greenSum = 0, blueSum = 0;
        foreach(Cell cell in aliveCells)
        {
            redSum += cell.cellColor.r;
            greenSum += cell.cellColor.g;
            blueSum += cell.cellColor.b;
        }
        if (aliveCells.Count != 0)
        {
            redSum = redSum / aliveCells.Count;
            greenSum = greenSum / aliveCells.Count;
            blueSum = blueSum / aliveCells.Count;
        }
        else
        {
            redSum = Random.Range(0, 1);
            greenSum = Random.Range(0, 1);
            blueSum = Random.Range(0, 1);
        }
        return new Color(redSum, greenSum, blueSum, 1f);
    }

    public bool isStable() {

        Simulation expirementalSimulation = new Simulation(this);
        int requireStepsForStability = 2;
        for(int i=0; i < requireStepsForStability; i++)
           expirementalSimulation.step();
        
        return expirementalSimulation == this;
    }

    public void step()
    {
        List<Cell> changedCells = this.getChangedCells();
        foreach(Cell cell in changedCells)
        {
            if (!cell.alive)
                this.deadCells++;
            this.setCell(cell);
        }
        this.generation++;
    }

    public static bool operator ==(Simulation obj1, Simulation obj2)
    {
        for(int i=0; i < obj1.cells.Length; i++)
        {
            if (obj1.cells[i] != obj2.cells[i])
                return false;
        }

        return true;
    }

    public static bool Equals(Simulation obj1, Simulation obj2)
    {
        return obj1 == obj2;
    }
    
    public static bool operator !=(Simulation obj1, Simulation obj2)
    {
        return !(obj1 == obj2);
    }
}