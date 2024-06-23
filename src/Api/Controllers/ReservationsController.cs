﻿using Application.Reservations.Commands.CreateReservation;
using Application.Reservations.Queries.GetReservationById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
public class ReservationsController(ISender _sender) : ApiController
{

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<IActionResult> Create([FromBody] CreateReservationCommand command)
	{
		var result = await _sender.Send(command);

		return result.Match(
			id => CreatedAtAction(nameof(GetById), new { id }, default),
			Problem);
	}

	[HttpGet("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
	{
		var result = await _sender.Send(
			new GetReservationByIdQuery(id),
			cancellationToken);

		return result.Match(Ok, Problem);
	}



}
