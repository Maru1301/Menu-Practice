﻿using BoardGame.Infrastractures;
using BoardGame.Models.EFModels;
using MongoDB.Bson;
using static BoardGame.Models.DTOs.GameDTOs;

namespace BoardGame.Services.Interfaces
{
    public interface IGameService
    {
        public Task<IEnumerable<GameDTO>> GetGameList();

        public Task<ObjectId> BeginNewGame(GameInfoDTO dto, string userAccount);

        public Task BeginNewRound(RoundInfoDTO dto);

        public Func<PlayerRoundInfo, PlayerRoundInfo, Result> MapRule(Character character);
    }
}
