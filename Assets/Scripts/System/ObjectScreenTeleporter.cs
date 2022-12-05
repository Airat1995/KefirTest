using KefirTest.System.Interface;
using KefirTest.View;
using UnityEngine;

namespace KefirTest.System
{
    public class ObjectScreenTeleporter : IUpdatableSystem
    {
        private readonly IMovableElement _player;
        private Bounds _cameraBounds;

        public ObjectScreenTeleporter(IMovableElement player, Bounds cameraBounds)
        {
            _player = player;
            _cameraBounds = cameraBounds;
        }

        public void Update(float deltaTime)
        {
            Vector2 playerPosition = _player.GetPosition();
            float newXPos = playerPosition.x;
            float newYPos = playerPosition.y;
            if (playerPosition.y - _player.GetSize().y / 2.0f > _cameraBounds.max.y)
                newYPos = _cameraBounds.min.y - _player.GetSize().y / 3.0f;
            if (playerPosition.y + _player.GetSize().y / 2.0f < _cameraBounds.min.y)
                newYPos = _cameraBounds.max.y + _player.GetSize().y / 3.0f;
            if (playerPosition.x - _player.GetSize().x / 2.0f > _cameraBounds.max.x)
                newXPos = _cameraBounds.min.x - _player.GetSize().x / 3.0f;
            if (playerPosition.x + _player.GetSize().x / 2.0f < _cameraBounds.min.x)
                newXPos = _cameraBounds.max.x - _player.GetSize().y / 3.0f;

            if (!Mathf.Approximately(playerPosition.y, newYPos) || !Mathf.Approximately(playerPosition.x, newXPos) )
                _player.SetPosition(new Vector2(newXPos, newYPos));
        }
    }
}