﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Pigeon.Enums;

namespace Pigeon.Models.DTOs;

public class TicketDTO
{
    public int Id { get; set; }

    public int LogRequestId { get; set; }

    public string AssignedTo { get; set; }

    public SystemFelowStatus Status { get; set; }

    public string? AdminComment { get; set; }

    public string? Result { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<SelectListItem> AvalibleStatus { get; set; }
}