﻿#pragma warning disable CS8618

namespace MilligramServer.Domain.Dtos;

public class FileDetailsDto
{
    public Guid Id { get; set; }
    public byte[] Content { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public bool IsImage { get; set; }
    public long SizeBytes { get; set; }
}