using System;
using System.Collections.Generic;
using System.Text;
using Library.Parser.Statements;

namespace Library.Parser
{
    public sealed class PParser
    {
        private static readonly IMatchable StructOrUnion = new PTokenAlternative(
            new PToken(PTokenType.Struct),
            new PToken(PTokenType.Union)
        );

        private static readonly IMatchable CompoundStatement = new PTokenEnclosed(new PToken("{"), new PToken("}"));

        private static readonly IMatchable StructOrUnionSpecifier = new PTokenAlternative(
            new PTokenSequence(StructOrUnion, new PToken(PTokenType.Identifier), CompoundStatement),
            new PTokenSequence(StructOrUnion, new PTokenEnclosed(new PToken("{"), new PToken("}"))),
            new PTokenSequence(StructOrUnion, new PToken(PTokenType.Identifier))
        );

        private static readonly IMatchable EnumSpecifier = new PTokenAlternative(
            new PTokenSequence(new PToken(PTokenType.Enum), CompoundStatement),
            new PTokenSequence(new PToken(PTokenType.Enum), new PToken(PTokenType.Identifier), CompoundStatement),
            new PTokenSequence(new PToken(PTokenType.Enum), new PToken(PTokenType.Identifier))
        );

        private static readonly IMatchable StorageClassSpecifier = new PTokenAlternative(
            new PToken(PTokenType.Typedef),
            new PToken(PTokenType.Extern),
            new PToken(PTokenType.Static),
            new PToken(PTokenType.Auto),
            new PToken(PTokenType.Register)
        );

        private static readonly IMatchable TypeSpecifier = new PTokenAlternative(
            new PToken(PTokenType.Void),
            new PToken(PTokenType.Char),
            new PToken(PTokenType.Short),
            new PToken(PTokenType.Int),
            new PToken(PTokenType.Long),
            new PToken(PTokenType.Float),
            new PToken(PTokenType.Double),
            new PToken(PTokenType.Signed),
            new PToken(PTokenType.Unsigned),
            StructOrUnionSpecifier,
            EnumSpecifier,
            new PToken(PTokenType.TypeName)
        );

        private static readonly IMatchable TypeQualifier = new PTokenAlternative(
            new PToken(PTokenType.Const),
            new PToken(PTokenType.Volatile)
        );

        private static readonly IMatchable DeclarationSpecifiersRepeat = new PTokenRepeat(
            new PTokenAlternative(
                StorageClassSpecifier,
                TypeSpecifier,
                TypeSpecifier,
                TypeQualifier
            ),
            true
        );

        private static readonly IMatchable DeclarationSpecifiers = new PTokenAlternative(
            StorageClassSpecifier,
            new PTokenSequence(StorageClassSpecifier, DeclarationSpecifiersRepeat),
            TypeSpecifier,
            new PTokenSequence(TypeSpecifier, DeclarationSpecifiersRepeat),
            TypeQualifier,
            new PTokenSequence(TypeQualifier, DeclarationSpecifiersRepeat)
        );

        private static readonly IMatchable TypeQualifierList = new PTokenRepeat(TypeQualifier, true);

        private static readonly IMatchable PointerRepeat = new PTokenRepeat(
            new PTokenAlternative(
                new PToken("*"),
                new PTokenSequence(new PToken("*"), TypeQualifierList)
            ),
            true
        );

        private static readonly IMatchable Pointer = new PTokenAlternative(
            new PToken("*"),
            new PTokenSequence(new PToken("*"), TypeQualifierList),
            new PTokenSequence(new PToken("*"), PointerRepeat),
            new PTokenSequence(new PToken("*"), TypeQualifierList, PointerRepeat)
        );

        private static readonly IMatchable DirectDeclarator = new PTokenSequence(
            new PToken(PTokenType.Identifier),
            new PTokenRepeat(
                new PTokenAlternative(
                    new PTokenEnclosed(new PToken("("), new PToken(")")),
                    new PTokenEnclosed(new PToken("["), new PToken("]"))
                ),
                true, 1
            )
        );

        private static readonly IMatchable Declarator = new PTokenAlternative(
            new PTokenSequence(Pointer, DirectDeclarator),
            DirectDeclarator
        );

        private static readonly IMatchable FunctionDeclarator = new PTokenAlternative(
            new PTokenSequence(DeclarationSpecifiers, Declarator),
            Declarator
        );

        private static readonly IMatchable JumpStatement = new PTokenAlternative(
            new PTokenSequence(new PToken(PTokenType.Goto), new PToken(PTokenType.Identifier), new PToken(";")),
            new PTokenSequence(new PToken(PTokenType.Continue), new PToken(";")),
            new PTokenSequence(new PToken(PTokenType.Break), new PToken(";")),
            new PTokenSequence(new PToken(PTokenType.Return), new PToken(";")),
            new PTokenSequence(new PToken(PTokenType.Return), new PTokenRepeat(PToken.AnyToken, false), new PToken(";"))
        );

        private static readonly IMatchable ValidExpressionTokens = PToken.AnyTokenBesides(
            PTokenType.Typedef,
            PTokenType.Ellipsis,
            PTokenType.Case,
            PTokenType.Default,
            PTokenType.If,
            PTokenType.Else,
            PTokenType.Switch,
            PTokenType.While,
            PTokenType.Do,
            PTokenType.For,
            PTokenType.Goto,
            PTokenType.Continue,
            PTokenType.Break,
            PTokenType.Return,
            PTokenType.LeftBrace,
            PTokenType.RightBrace,
            PTokenType.Semicolon
        );

        private static readonly IMatchable ExpressionRepeat = new PTokenRepeat(ValidExpressionTokens, false);

        private static readonly IMatchable LabeledStatement = new PTokenAlternative(
            new PTokenSequence(new PToken(PTokenType.Identifier), new PToken(":")),
            new PTokenSequence(
                new PToken(PTokenType.Case),
                new PTokenRepeat(ValidExpressionTokens, false, 0, (tokens, startIndex, numMatched) =>
                {
                    int i, countQueries = 0, countColons = 0;

                    for (i = startIndex; i - startIndex < numMatched; i++)
                        if (tokens[i].TokenCode == "?")
                            countQueries++;
                        else if (tokens[i].TokenCode == ":")
                            countColons++;

                    return countColons == countQueries + 1 && tokens[i - 1].TokenCode == ":";
                })
            ),
            new PTokenSequence(new PToken(PTokenType.Default), new PToken(":"))
        );

        private static readonly IMatchable SelectionStatement = new PTokenAlternative(
            new PTokenSequence(new PToken(PTokenType.If), new PTokenEnclosed(new PToken("("), new PToken(")"))),
            new PToken(PTokenType.Else),
            new PTokenSequence(new PToken(PTokenType.Switch), new PTokenEnclosed(new PToken("("), new PToken(")")))
        );

        private static readonly IMatchable IterationStatement = new PTokenAlternative(
            new PToken(PTokenType.Do),
            new PTokenSequence(new PToken(PTokenType.While), new PTokenEnclosed(new PToken("("), new PToken(")"))),
            new PTokenSequence(new PToken(PTokenType.For), new PTokenEnclosed(new PToken("("), new PToken(")")))
        );

        private static readonly IMatchable ExpressionStatement = new PTokenSequence(ExpressionRepeat, new PToken(";"));

        private static readonly IMatchable Statement = new PTokenAlternative(
            LabeledStatement,
            CompoundStatement,
            SelectionStatement,
            IterationStatement,
            JumpStatement,
            ExpressionStatement
        );

        private readonly List<SJumpStatement> _breakJumps = new List<SJumpStatement>();
        private readonly List<SLabeledStatement> _caseLabels = new List<SLabeledStatement>();
        private readonly List<SJumpStatement> _continueJumps = new List<SJumpStatement>();
        private readonly List<SJumpStatement> _gotoJumps = new List<SJumpStatement>();


        private readonly List<SLabeledStatement> _labels = new List<SLabeledStatement>();
        private readonly List<SJumpStatement> _returnJumps = new List<SJumpStatement>();

        public static IEnumerable<SFunctionDefinition> GetFunctionContents(PToken[] tokens)
        {
            foreach (var matched in MatchExpression(FunctionDeclarator, tokens))
            {
                var currResult = CompoundStatement.match(tokens, matched.StartIndex + matched.NumMatches);

                if (!currResult.IsMatch || currResult.NumMatches <= 0) continue;
                int i;

                for (i = matched.StartIndex; i < tokens.Length && tokens[i].TokenCode != "("; i++) ;

                var functionName = tokens[i - 1].TokenCode;

                yield return new SFunctionDefinition(functionName,
                    new PParser().ParseCompoundStatement(
                        new Queue<PMatchedData>(
                            new[]
                            {
                                new PMatchedData(tokens, matched.StartIndex + matched.NumMatches, currResult.NumMatches)
                            }
                        )
                    )
                );
            }
        }

        public SConditionalStatement ParseIfStatement(Queue<PMatchedData> matchedData)
        {
            var currTokens = matchedData.Dequeue().GetTokens();

            if (currTokens[0].TokenType != PTokenType.If)
                throw new ArgumentException("matchedData should start with an if statement!", nameof(matchedData));

            var conditionExpression = new SExpression(GetText(currTokens, 2, currTokens.Length - 1));
            var ifTrueStatement = ParseStatement(matchedData);

            if (ifTrueStatement.NextStatement is SConditionalStatement statement && statement.Condition == null)
            {
                var elseStatement = statement.IfTrueStatement;

                return new SConditionalStatement(conditionExpression, ifTrueStatement, elseStatement,
                    ifTrueStatement.NextStatement.NextStatement);
            }

            return new SConditionalStatement(conditionExpression, ifTrueStatement, ifTrueStatement.NextStatement);
        }

        private static string GetText(PToken[] tokens, int startIndex, int endIndex)
        {
            var builder = new StringBuilder();
            int i;

            for (i = startIndex; i < endIndex; i++)
                builder.Append(tokens[i].TokenCode + " ");

            return builder.ToString().TrimEnd();
        }

        private static IEnumerable<PMatchedData> MatchCompoundStatementContents(PToken[] tokens)
        {
            if (tokens[0].TokenCode != "{" || tokens[tokens.Length - 1].TokenCode != "}")
                throw new ArgumentException("tokens should represent a compound statement!", nameof(tokens));

            return MatchExpression(Statement, tokens, true, 1);
        }

        private static IEnumerable<PMatchedData> MatchExpression(IMatchable expression, PToken[] tokens,
            bool getFirstMatches = true, int startIndex = 0)
        {
            int i;
            MatchResult currResult;

            for (i = startIndex;
                i < tokens.Length;
                i += getFirstMatches && currResult.IsMatch ? currResult.NumMatches : 1)
            {
                currResult = expression.match(tokens, i);

                if (!currResult.IsMatch || currResult.NumMatches <= 0) continue;
                yield return new PMatchedData(tokens, i, currResult.NumMatches);
            }
        }

        private SCompoundStatement ParseCompoundStatement(Queue<PMatchedData> matchedData)
        {
            var containedStatements = new List<SStatement>();

            var currTokens = matchedData.Dequeue().GetTokens();

            var containedMatchedData =
                new Queue<PMatchedData>(MatchCompoundStatementContents(currTokens));

            SStatement currStatement;

            containedStatements.Add(currStatement = ParseStatement(containedMatchedData));

            while (currStatement != null)
                containedStatements.Add(currStatement = currStatement.NextStatement);

            return new SCompoundStatement(containedStatements.ToArray(), ParseStatement(matchedData));
        }

        private SCompoundStatement ParseDoWhileStatement(Queue<PMatchedData> matchedData)
        {
            var currTokens = matchedData.Dequeue().GetTokens();
            SStatement nextStatement;
            int i;

            if (currTokens[0].TokenType != PTokenType.Do)
                throw new ArgumentException("matchedData should start with a do-while statement!", nameof(matchedData));

            var loopStatement = ParseStatement(matchedData);
            loopStatement.Visible = false;

            if (loopStatement.NextStatement is SLabeledStatement &&
                loopStatement.NextStatement.NextStatement is SLabeledStatement)
                nextStatement = ((SLabeledStatement) loopStatement.NextStatement.NextStatement).LabeledStatement;
            else
                throw new ArgumentException("incorrect do-while statement!", nameof(matchedData));

            var breakTarget =
                new SLabeledStatement("do-while-loop: break target", null, nextStatement) {Visible = false};
            var continueLoop = new SJumpStatement("continue", null) {Visible = false};
            var continueTarget =
                new SLabeledStatement("do-while-loop: continue target", null, loopStatement) {Visible = false};

            continueLoop.SetTargetStatement(continueTarget);

            var statement = new SConditionalStatement(
                ((SConditionalStatement) ((SLabeledStatement) loopStatement.NextStatement).LabeledStatement).Condition,
                continueLoop,
                null
            );
            loopStatement.SetNextStatement(
                statement
            );

            for (i = 0; i < _breakJumps.Count; i++)
                if (continueTarget.Contains(_breakJumps[i]))
                {
                    _breakJumps[i].SetTargetStatement(breakTarget);
                    _breakJumps.RemoveAt(i);
                    i--;
                }

            for (i = 0; i < _continueJumps.Count; i++)
                if (continueTarget.Contains(_continueJumps[i]))
                {
                    _continueJumps[i].SetTargetStatement(continueTarget);
                    _continueJumps.RemoveAt(i);
                    i--;
                }

            var statements = new[] {continueTarget, loopStatement.NextStatement};

            return new SCompoundStatement(
                statements, breakTarget
            );
        }

        private SConditionalStatement ParseElseStatement(Queue<PMatchedData> matchedData)
        {
            var currTokens = matchedData.Dequeue().GetTokens();

            if (currTokens[0].TokenType != PTokenType.Else)
                throw new ArgumentException("matchedData should start with an else statement!", nameof(matchedData));

            var ifTrueStatement = ParseStatement(matchedData);

            return new SConditionalStatement(null, ifTrueStatement, ifTrueStatement.NextStatement);
        }

        private SExpressionStatement ParseExpressionStatement(Queue<PMatchedData> matchedData)
        {
            var currTokens = matchedData.Dequeue().GetTokens();
            var currExpression = new SExpression(GetText(currTokens, 0, currTokens.Length - 1));
            return new SExpressionStatement(currExpression, ParseStatement(matchedData));
        }

        private SCompoundStatement ParseForStatement(Queue<PMatchedData> matchedData)
        {
            var currTokens = matchedData.Dequeue().GetTokens();

            int i, j;

            if (currTokens[0].TokenType != PTokenType.For)
                throw new ArgumentException("matchedData should start with a for statement!", nameof(matchedData));

            for (i = 0; i < currTokens.Length && currTokens[i].TokenCode != ";"; i++) ;
            var startExpression = new SExpression(GetText(currTokens, 2, i));

            for (j = i + 1; j < currTokens.Length && currTokens[j].TokenCode != ";"; j++) ;
            var conditionExpression = new SExpression(GetText(currTokens, i + 1, j));
            var iterationExpression = new SExpression(GetText(currTokens, j + 1, currTokens.Length - 1));
            var loopStatement = ParseStatement(matchedData);
            var nextStatement = loopStatement.NextStatement;

            var breakTarget = new SLabeledStatement("for-loop: break target", null, nextStatement) {Visible = false};
            var continueLoop = new SJumpStatement("for-loop: next iteration", null) {Visible = false};
            var iteration = new SExpressionStatement(iterationExpression, continueLoop) {Visible = false};
            var continueTarget = new SLabeledStatement("for-loop: continue target", null, iteration) {Visible = false};
            loopStatement.SetNextStatement(continueTarget);


            var iterationStart = new SLabeledStatement("for-loop: iteration start", null, new SConditionalStatement(
                    conditionExpression,
                    new SCompoundStatement(new[]
                    {
                        loopStatement,
                        continueTarget,
                        continueLoop
                    }, null),
                    null
                ))
                {Visible = false};

            continueLoop.SetTargetStatement(iterationStart);

            for (i = 0; i < _breakJumps.Count; i++)
                if (loopStatement.Contains(_breakJumps[i]))
                {
                    _breakJumps[i].SetTargetStatement(breakTarget);
                    _breakJumps.RemoveAt(i);
                    i--;
                }

            for (i = 0; i < _continueJumps.Count; i++)
                if (loopStatement.Contains(_continueJumps[i]))
                {
                    _continueJumps[i].SetTargetStatement(continueTarget);
                    _continueJumps.RemoveAt(i);
                    i--;
                }

            var cExpression = new SExpressionStatement(startExpression, iterationStart) {Visible = false};

            return new SCompoundStatement(
                new SStatement[]
                {
                    cExpression,
                    iterationStart
                },
                breakTarget);
        }

        private SJumpStatement ParseJumpStatement(Queue<PMatchedData> matchedData)
        {
            var currTokens = matchedData.Dequeue().GetTokens();
            SJumpStatement result;

            if (currTokens[0].TokenType == PTokenType.Return)
            {
                if (currTokens.Length > 1)
                {
                    var returnExpression = new SExpression(GetText(currTokens, 1, currTokens.Length - 1));
                    result = new SJumpStatement(returnExpression, ParseStatement(matchedData));
                }
                else
                {
                    result = new SJumpStatement("return", ParseStatement(matchedData));
                }

                result.Visible = false;
                _returnJumps.Add(result);
            }
            else
            {
                result = new SJumpStatement(GetText(currTokens, 0, currTokens.Length - 1), ParseStatement(matchedData));

                if (currTokens[0].TokenType == PTokenType.Goto)
                {
                    foreach (var statement in _labels)
                        if (statement.CodeString == result.TargetIdentifier)
                        {
                            result.SetTargetStatement(statement);

                            return result;
                        }

                    _gotoJumps.Add(result);
                }
                else if (currTokens[0].TokenType == PTokenType.Break)
                {
                    _breakJumps.Add(result);
                }
                else if (currTokens[0].TokenType == PTokenType.Continue)
                {
                    _continueJumps.Add(result);
                }
                else
                {
                    throw new ArgumentException("matchedData does not represent a valid jump statement!",
                        nameof(matchedData));
                }
            }

            result.Visible = false;
            return result;
        }

        private SLabeledStatement ParseLabeledStatement(Queue<PMatchedData> matchedData)
        {
            var currTokens = matchedData.Dequeue().GetTokens();
            var labelName = currTokens[0].TokenCode;
            SExpression caseExpression = null;

            var labeled = ParseStatement(matchedData);

            if (labelName == "case")
                caseExpression = new SExpression(GetText(currTokens, 1, currTokens.Length - 1));

            if (labeled == null)
                throw new InvalidOperationException("error: label at end of compound statement!");

            var result = new SLabeledStatement(labelName, caseExpression, labeled);

            if (labelName == "case" || labelName == "default")
            {
                _caseLabels.Add(result);
            }
            else
            {
                int i;

                for (i = 0; i < _gotoJumps.Count; i++)
                    if (_gotoJumps[i].TargetIdentifier == labelName)
                    {
                        _gotoJumps[i].SetTargetStatement(result);
                        _gotoJumps.RemoveAt(i);

                        return result;
                    }

                _labels.Add(result);
            }

            return result;
        }

        private SStatement ParseStatement(Queue<PMatchedData> matchedData)
        {
            if (matchedData.Count == 0)
                return null;

            var currMatched = matchedData.Peek();

            switch (currMatched[0].TokenType)
            {
                case PTokenType.If:
                    return ParseIfStatement(matchedData);

                case PTokenType.Else:
                    return ParseElseStatement(matchedData);

                case PTokenType.Switch:
                    return ParseSwitchStatement(matchedData);

                case PTokenType.While:
                    return ParseWhileStatement(matchedData);

                case PTokenType.Do:
                    return ParseDoWhileStatement(matchedData);

                case PTokenType.For:
                    return ParseForStatement(matchedData);

                case PTokenType.Goto:
                case PTokenType.Return:
                case PTokenType.Continue:
                case PTokenType.Break:
                    return ParseJumpStatement(matchedData);

                case PTokenType.LeftBrace:
                    return ParseCompoundStatement(matchedData);
            }

            if (currMatched[currMatched.NumMatches - 1].TokenCode == ":")
                return ParseLabeledStatement(matchedData);

            return ParseExpressionStatement(matchedData);
        }

        private SSwitchStatement ParseSwitchStatement(Queue<PMatchedData> matchedData)
        {
            var currTokens = matchedData.Dequeue().GetTokens();

            int i;

            if (currTokens[0].TokenType != PTokenType.Switch)
                throw new ArgumentException("matchedData should start with a switch statement!", nameof(matchedData));

            var conditionExpression = new SExpression(GetText(currTokens, 2, currTokens.Length - 1));
            var switchCases = new List<SLabeledStatement>();
            var switchStatement = ParseStatement(matchedData);
            var nextStatement = switchStatement.NextStatement;

            for (i = 0; i < _caseLabels.Count; i++)
                if (switchStatement.Contains(_caseLabels[i]))
                {
                    _caseLabels[i].Visible = false;

                    switchCases.Add(_caseLabels[i]);
                    _caseLabels.RemoveAt(i);
                    i--;
                }

            var result = new SSwitchStatement(conditionExpression, switchStatement,
                switchCases.ToArray(), nextStatement);

            for (i = 0; i < _breakJumps.Count; i++)
                if (switchStatement.Contains(_breakJumps[i]))
                {
                    _breakJumps[i].Visible = false;
                    _breakJumps[i].SetTargetStatement(result.NextStatement as SLabeledStatement);
                    _breakJumps.RemoveAt(i);
                    i--;
                }

            return result;
        }

        private SLabeledStatement ParseWhileStatement(Queue<PMatchedData> matchedData)
        {
            var currTokens = matchedData.Dequeue().GetTokens();

            int i;

            if (currTokens[0].TokenType != PTokenType.While)
                throw new ArgumentException("matchedData should start with a while statement!", nameof(matchedData));

            var conditionExpression = new SExpression(GetText(currTokens, 2, currTokens.Length - 1));
            var loopStatement = ParseStatement(matchedData);
            var nextStatement = loopStatement.NextStatement;

            var breakTarget = new SLabeledStatement("while-loop: break target", null, nextStatement) {Visible = false};

            var continueLoop = new SJumpStatement("continue", null);
            loopStatement.SetNextStatement(continueLoop);
            continueLoop.Visible = false;

            var continueTarget = new SLabeledStatement("while-loop: continue target", null, new SConditionalStatement(
                    conditionExpression,
                    new SCompoundStatement(new[] {loopStatement, continueLoop}, null),
                    breakTarget
                ))
                {Visible = false};

            continueLoop.SetTargetStatement(continueTarget);

            for (i = 0; i < _breakJumps.Count; i++)
                if (continueTarget.Contains(_breakJumps[i]))
                {
                    _breakJumps[i].SetTargetStatement(breakTarget);
                    _breakJumps.RemoveAt(i);
                    i--;
                }

            for (i = 0; i < _continueJumps.Count; i++)
                if (continueTarget.Contains(_continueJumps[i]))
                {
                    _continueJumps[i].SetTargetStatement(continueTarget);
                    _continueJumps.RemoveAt(i);
                    i--;
                }

            return continueTarget;
        }
    }
}