﻿namespace Library.Parser
{
    public enum PTokenType
    {
        Identifier,
        Constant,
        StringLiteral,
        Sizeof,
        PtrOp,
        IncOp,
        DecOp,
        LeftOp,
        RightOp,
        LeOp,
        GeOp,
        EqOp,
        NeOp,
        AndOp,
        OrOp,
        MulAssign,
        DivAssign,
        ModAssign,
        AddAssign,
        SubAssign,
        LeftAssign,
        RightAssign,
        AndAssign,
        XorAssign,
        OrAssign,
        TypeName,

        Typedef,
        Extern,
        Static,
        Auto,
        Register,
        Char,
        Short,
        Int,
        Long,
        Signed,
        Unsigned,
        Float,
        Double,
        Const,
        Volatile,
        Void,
        Struct,
        Union,
        Enum,
        Ellipsis,

        Case,
        Default,
        If,
        Else,
        Switch,
        While,
        Do,
        For,
        Goto,
        Continue,
        Break,
        Return,

        Semicolon,
        LeftBrace,
        RightBrace,
        Comma,
        Colon,
        AssignOp,
        LeftParen,
        RightParen,
        LeftBracket,
        RightBracket,
        InstOp,
        BitAndOp,
        NotOp,
        BitNotOp,
        SubOp,
        AddOp,
        MulOp,
        DivOp,
        ModOp,
        LtOp,
        GtOp,
        XorOp,
        BitOrOp,
        Query,

        Comment,

        Eof
    }
}