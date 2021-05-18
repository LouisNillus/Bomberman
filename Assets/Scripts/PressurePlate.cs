using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Animator _animator;
    Transform _self;
    bool active = true;

    void Start()
    {
        _self = transform;
    }

    void Update()
    {
        if (!active) return;
        
        Cell cell = GridHandler.instance.GetCellFromPos(_self.position);
        foreach (Player player in GridHandler.instance.players)
        {
            if (GridHandler.instance.GetCellFromPos(player.transform.position) == cell)
            {
                _animator.Play("PressurePlateOn");
                var walls = GridHandler.instance.GetAllCellsOfType(EntityType.Wall);
                var selectedWalls = GridHandler.instance.GetRandomCells(walls, (int) (walls.Count * 0.25f));
                foreach (Cell wall in selectedWalls)
                {
                    Destroy(wall.entity.gameObject);
                }

                active = false;
            }

            if (!active) break;
        }
    }
}