﻿namespace Hafnia.DTOs;

public record Tag
(
    string Id,
    string Name,
    string[] ChildrenTags
);
